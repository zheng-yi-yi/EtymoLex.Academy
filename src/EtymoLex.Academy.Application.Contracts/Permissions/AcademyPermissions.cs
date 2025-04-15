namespace EtymoLex.Academy.Permissions;

public static class AcademyPermissions
{
    public const string GroupName = "Academy";


    
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public class Morpheme
    {
        public const string Default = "Morpheme";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Copy = Default + ".Copy";
        public const string Import = Default + ".Import";
        public const string Export = Default + ".Export";
        public const string Delete = Default + ".Delete";
    }
}
