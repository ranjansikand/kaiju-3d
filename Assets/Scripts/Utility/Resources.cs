// Handles a set of resources


[System.Serializable]
public class Resources {
    public int gold;
    public int wood;

    public Resources(int x, int y) {
        gold = x;
        wood = y;
    }

    #region Math
    public static Resources operator +(Resources x, Resources y) {
        return new Resources(x.gold + y.gold, x.wood + y.wood);
    }

    public static Resources operator -(Resources x, Resources y) {
        return new Resources(x.gold - y.gold, x.wood - y.wood);
    }

    public static Resources operator *(Resources x, int y) {
        return new Resources(x.gold * y, x.wood * y);
    }
    #endregion

    #region Less
    public static bool operator <(Resources x, Resources y) {
        if (x.gold < y.gold || x.wood < y.wood) return true;
        else return false;
    }    
    
    public static bool operator <(Resources x, int y) {
        if (x.gold < y || x.wood < y) return true;
        else return false;
    }

    public static bool operator <=(Resources x, Resources y) {
        if (x.gold <= y.gold || x.wood <= y.wood) return true;
        else return false;
    }
    #endregion


    #region Greater
    public static bool operator >(Resources x, Resources y) {
        if (x.gold > y.gold && x.wood > y.wood) return true;
        else return false;
    }
    
    public static bool operator >(Resources x, int y) {
        if (x.gold > y && x.wood > y) return true;
        else return false;
    }

    public static bool operator >=(Resources x, Resources y) {
        if (x.gold >= y.gold && x.wood >= y.wood) return true;
        else return false;
    }
    #endregion

    #region Equals
    public override bool Equals(object obj) {
        if (!(obj is Resources)) return false;

        return (gold == ((Resources)obj).gold && wood == ((Resources)obj).wood);
    }
    
    public override int GetHashCode() {
        throw new System.NotImplementedException();
    }

    public static bool operator ==(Resources x, Resources y) {
        if (x.gold == y.gold && x.wood == y.wood) return true;
        else return false;
    }

    public static bool operator !=(Resources x, Resources y) {
        if (x.gold == y.gold && x.wood == y.wood) return false;
        else return true;
    }

    public static bool operator ==(Resources x, int y) {
        return (x.gold == y && x.wood == y);
    }

    public static bool operator !=(Resources x, int y) {
        return (x.gold != y || x.wood != y);
    }
    #endregion
}