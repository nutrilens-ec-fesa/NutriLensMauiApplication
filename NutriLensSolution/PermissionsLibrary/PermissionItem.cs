namespace PermissionsLibrary
{
    public class PermissionItem
    {
        public PermissionType Type;
        public string Header { get; set; }
        public string Reason { get; set; }

        public PermissionItem(PermissionType type, string header, string reason)
        {
            Type = type;
            Header = header;
            Reason = reason;
        }
    }
}
