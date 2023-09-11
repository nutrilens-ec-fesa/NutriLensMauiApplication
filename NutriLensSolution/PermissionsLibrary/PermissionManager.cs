using PopupLibrary;

namespace PermissionsLibrary
{
    // All the code in this file is included in all platforms.

    public enum PermissionType
    {
        CalendarRead,
        CalendarWrite,
        Câmera,
        ContactsRead,
        ContactsWrite,
        Flashlight,
        LocationWhenInUse,
        LocationAlways,
        Media,
        Microfone,
        Phone,
        Photos,
        Reminders,
        Sensors,
        Sms,
        Speech,
        StorageRead,
        StorageWrite
    }

    public interface IPermissionManager
    {
        public bool AllGranted { get; }
        public Task CheckPermissionsStatus();
        public Task CheckAndRequestPermissionsStatus();
        public void SetNeededPermissions(List<PermissionItem> permissionItems);

        public event DelegatePopupFullyPersonalized EventInfoMessage;
    }

    public class PermissionManager : IPermissionManager
    {
        private List<PermissionItem> _neededPermissions;
        public event DelegatePopupFullyPersonalized EventInfoMessage;

        public Dictionary<PermissionType, PermissionStatus> PermissionsStatus { get; set; }
        public bool AllGranted { get => PermissionsStatus.Where(x => x.Value == PermissionStatus.Granted).Count() == PermissionsStatus.Count; }
        public PermissionManager()
        {
            PermissionsStatus = new Dictionary<PermissionType, PermissionStatus>();
        }
        public PermissionManager(List<PermissionItem> neededPermissions)
        {
            _neededPermissions = neededPermissions;
            PermissionsStatus = new Dictionary<PermissionType, PermissionStatus>();
        }

        public void SetNeededPermissions(List<PermissionItem> permissionItems)
        {
            _neededPermissions = permissionItems;
        }

        public async Task CheckPermissionsStatus()
        {
            foreach (PermissionItem permission in _neededPermissions)
            {
                PermissionStatus permissionStatus = await GetPermissionStatus(permission.Type);
                PermissionsStatus.TryAdd(permission.Type, permissionStatus);
            }
        }

        public async Task CheckAndRequestPermissionsStatus()
        {
            foreach (PermissionItem permission in _neededPermissions)
            {
                // Solicita o status da permissão
                PermissionStatus permissionStatus = await GetPermissionStatus(permission.Type);

                // Se a permissão não estiver liberada
                if (permissionStatus != PermissionStatus.Granted)
                {
                    // Exibe mensagem do motivo da permissão
                    await EventInfoMessage?.Invoke(permission.Header, permission.Reason, "OK", string.Empty, string.Empty);

                    // Solicita a permissão de forma nativa do Android (normalmente o prompt só funciona na primeira vez)
                    permissionStatus = await RequestPermission(permission.Type);
                }

                // Se ainda assim não tiver sido liberada, permanece em loop até que o usuário aceite
                while (permissionStatus != PermissionStatus.Granted)
                {
                    // Chama menu de 'Configurações'
                    AppInfo.Current.ShowSettingsUI();

                    // Exibe mensagem do motivo da permissão
                    await EventInfoMessage?.Invoke(permission.Header, permission.Reason, "OK", string.Empty, string.Empty);

                    // Solicita o status da permissão
                    permissionStatus = await GetPermissionStatus(permission.Type);
                }

                // Adiciona a permissão no dicionário
                PermissionsStatus.TryAdd(permission.Type, permissionStatus);
            }
        }

        private async Task<PermissionStatus> RequestPermission(PermissionType type)
        {
            return type switch
            {
                PermissionType.CalendarRead => await Permissions.RequestAsync<Permissions.CalendarRead>(),
                PermissionType.CalendarWrite => await Permissions.RequestAsync<Permissions.CalendarWrite>(),
                PermissionType.Câmera => await Permissions.RequestAsync<Permissions.Camera>(),
                PermissionType.ContactsRead => await Permissions.RequestAsync<Permissions.ContactsRead>(),
                PermissionType.ContactsWrite => await Permissions.RequestAsync<Permissions.ContactsWrite>(),
                PermissionType.Flashlight => await Permissions.RequestAsync<Permissions.Flashlight>(),
                PermissionType.LocationWhenInUse => await Permissions.RequestAsync<Permissions.LocationWhenInUse>(),
                PermissionType.LocationAlways => await Permissions.RequestAsync<Permissions.LocationAlways>(),
                PermissionType.Media => await Permissions.RequestAsync<Permissions.Media>(),
                PermissionType.Microfone => await Permissions.RequestAsync<Permissions.Microphone>(),
                PermissionType.Phone => await Permissions.RequestAsync<Permissions.Phone>(),
                PermissionType.Photos => await Permissions.RequestAsync<Permissions.Photos>(),
                PermissionType.Reminders => await Permissions.RequestAsync<Permissions.Reminders>(),
                PermissionType.Sensors => await Permissions.RequestAsync<Permissions.Sensors>(),
                PermissionType.Sms => await Permissions.RequestAsync<Permissions.Sms>(),
                PermissionType.Speech => await Permissions.RequestAsync<Permissions.Speech>(),
                PermissionType.StorageRead => await Permissions.RequestAsync<Permissions.StorageRead>(),
                PermissionType.StorageWrite => await Permissions.RequestAsync<Permissions.StorageWrite>(),
                _ => PermissionStatus.Unknown,
            };
        }

        private async Task<PermissionStatus> GetPermissionStatus(PermissionType type)
        {
            return type switch
            {
                PermissionType.CalendarRead => await Permissions.CheckStatusAsync<Permissions.CalendarRead>(),
                PermissionType.CalendarWrite => await Permissions.CheckStatusAsync<Permissions.CalendarWrite>(),
                PermissionType.Câmera => await Permissions.CheckStatusAsync<Permissions.Camera>(),
                PermissionType.ContactsRead => await Permissions.CheckStatusAsync<Permissions.ContactsRead>(),
                PermissionType.ContactsWrite => await Permissions.CheckStatusAsync<Permissions.ContactsWrite>(),
                PermissionType.Flashlight => await Permissions.CheckStatusAsync<Permissions.Flashlight>(),
                PermissionType.LocationWhenInUse => await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>(),
                PermissionType.LocationAlways => await Permissions.CheckStatusAsync<Permissions.LocationAlways>(),
                PermissionType.Media => await Permissions.CheckStatusAsync<Permissions.Media>(),
                PermissionType.Microfone => await Permissions.CheckStatusAsync<Permissions.Microphone>(),
                PermissionType.Phone => await Permissions.CheckStatusAsync<Permissions.Phone>(),
                PermissionType.Photos => await Permissions.CheckStatusAsync<Permissions.Photos>(),
                PermissionType.Reminders => await Permissions.CheckStatusAsync<Permissions.Reminders>(),
                PermissionType.Sensors => await Permissions.CheckStatusAsync<Permissions.Sensors>(),
                PermissionType.Sms => await Permissions.CheckStatusAsync<Permissions.Sms>(),
                PermissionType.Speech => await Permissions.CheckStatusAsync<Permissions.Speech>(),
                PermissionType.StorageRead => await Permissions.CheckStatusAsync<Permissions.StorageRead>(),
                PermissionType.StorageWrite => await Permissions.CheckStatusAsync<Permissions.StorageWrite>(),
                _ => PermissionStatus.Unknown,
            };
        }
    }
}