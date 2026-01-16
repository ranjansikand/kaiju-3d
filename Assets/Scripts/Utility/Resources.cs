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
    
    // public override bool Equals(object obj)
    // {
    //     //
    //     // See the full list of guidelines at
    //     //   http://go.microsoft.com/fwlink/?LinkID=85237
    //     // and also the guidance for operator== at
    //     //   http://go.microsoft.com/fwlink/?LinkId=85238
    //     //
        
    //     if (obj == null || GetType() != obj.GetType() {
    //         return false;
    //     }
        
    //     // TODO: write your implementation of Equals() here
    //     return base.Equals (obj);
    // }
    
    // // override object.GetHashCode
    // public override int GetHashCode()
    // {
    //     // TODO: write your implementation of GetHashCode() here
    //     throw new System.NotImplementedException();
    //     return base.GetHashCode();
    // }

    // public static bool operator ==(Resources x, Resources y) {
    //     if (x.gold == y.gold && x.wood == y.wood) return true;
    //     else return false;
    // }

    // public static bool operator !=(Resources x, Resources y) {
    //     if (x.gold == y.gold && x.wood == y.wood) return false;
    //     else return true;
    // }
    #endregion
}