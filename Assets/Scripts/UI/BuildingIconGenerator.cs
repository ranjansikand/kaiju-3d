// Creates a mesh, uses camera to capture to render texture, then generates a sprite


using UnityEngine;
using System.Collections.Generic;

public class BuildingIconGenerator : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;
    [SerializeField] private Light renderLight;
    [SerializeField] private int iconSize = 256;
    
    private Dictionary<Building, Sprite> iconCache = new Dictionary<Building, Sprite>();
    
    void Awake() {
        // Set up render camera
        if (renderCamera == null) {
            GameObject camObj = new GameObject("IconRenderCamera");
            renderCamera = camObj.AddComponent<Camera>();
            camObj.transform.SetParent(transform);
        }
        
        renderCamera.enabled = false; // Only render when we need it
        renderCamera.orthographic = true;
        renderCamera.backgroundColor = new Color(0, 0, 0, 0); // Transparent background
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        
        // Set up light if needed
        // if (renderLight == null) {
        //     GameObject lightObj = new GameObject("IconRenderLight");
        //     renderLight = lightObj.AddComponent<Light>();
        //     lightObj.transform.SetParent(transform);
        //     renderLight.type = LightType.Directional;
        //     renderLight.transform.rotation = Quaternion.Euler(45, -30, 0);
        // }
    }
    
    // Gets an icon for a building. Returns cached version if available, generates if not.
    public Sprite GetIcon(Building building) {
        // Check cache first
        if (iconCache.ContainsKey(building)) {
            return iconCache[building];
        }
        
        // Generate new icon
        Sprite icon = GenerateIcon(building);
        iconCache[building] = icon;
        return icon;
    }

    // Generates a sprite icon from a building's mesh
    Sprite GenerateIcon(Building building) {
        if (building.mesh == null) {
            Debug.LogWarning($"Building {building.buildingName} has no mesh!");
            return null;
        }
        
        // Step 1: Create temporary preview object
        GameObject tempObj = new GameObject("IconPreview");
        MeshFilter mf = tempObj.AddComponent<MeshFilter>();
        MeshRenderer mr = tempObj.AddComponent<MeshRenderer>();

        mf.mesh = building.mesh;
        mr.material = building.material;
        
        // Step 2: Position the object at origin with nice angle
        tempObj.transform.position = new Vector3(0, 30, 0);
        tempObj.transform.rotation = Quaternion.Euler(0, 0, 0); // 3/4 view
        
        // Step 3: Calculate camera position to frame the mesh
        Bounds bounds = building.mesh.bounds;
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        float distance = maxSize * 3f; // How far back the camera sits
        
        renderCamera.transform.position = new Vector3(distance, distance * 1.5f + 30, -distance);
        renderCamera.transform.LookAt(tempObj.transform.position);
        renderCamera.orthographicSize = maxSize * 0.6f; // Zoom level
        
        // Step 4: Create render texture (the "photo paper")
        RenderTexture rt = new RenderTexture(iconSize, iconSize, 24);
        rt.antiAliasing = 4; // Smooth edges
        renderCamera.targetTexture = rt;
        
        // Step 5: Take the picture
        renderCamera.Render();
        
        // Step 6: Copy RenderTexture to Texture2D (GPU to CPU)
        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        
        // Step 7: Convert Texture2D to Sprite for UI
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, iconSize, iconSize),
            new Vector2(0.5f, 0.5f), // Pivot at center
            100f // Pixels per unit
        );
        
        // Step 8: Clean up temporary objects
        renderCamera.targetTexture = null;
        Destroy(rt);
        Destroy(tempObj);
        
        return sprite;
    }
    
    /// Clears all cached icons and frees memory
    public void ClearCache() {
        foreach (var sprite in iconCache.Values) {
            if (sprite != null) {
                Destroy(sprite.texture);
                Destroy(sprite);
            }
        }
        iconCache.Clear();
    }
}