namespace EtymoLex.Academy;

public static class UFEConsts
{
    public const string DbTablePrefix = "mlx_";

    public const string DbSchema = null;

    public const string ModelSearchField = "Name";
    public const string TenantIdField = "TenantId";
    public const string DisplayNameField = "DisplayName";
    public const string DescriptionField = "Description";
    public const string MenuNotExisted = "MenuNotExisted";
    public const string RoleNotExisted = "RoleNotExisted";
    public const string UserNotExisted = "UserNotExisted";
    public const string SupportTeamNotExisted = "SupportTeamNotExisted";
    public const string ChildEntityExists = "ChildEntityExists";
    public const string INITPASSWORD = "pass!@word2133WA";
    public const string INITEmail = "pass@tenant.com";
    public const string TextTemplateNotExisted = "TextTemplateNotExisted";
    public const string LastModificationTimeField = "LastModificationTime";
    public const string CreateTimeField = "CreationTime";
    public const string SendMessage = "SendMessage";
    public const string DefaultFilter = "%";
    public const string GetListAction = "GetList";

    public const string ContentTemplateCacheKey = "ContentTemplateCache";
    public const string NotificationSettingCacheKey = "NotificationSettingCache";
    public const string DefaultEmailSubject = "Mosaic Notification";
    public const string Email = "Email";
    public const string InApp = "In-App";
    public const string RecordCreator = "Record Creator";
    public const string RecordExecutor = "Record Executor";
    public const string APIDefineList = "API Define List";

    #region next action
    public const string NotFound = "Please verify the input and try again. If it still does not exist, consider creating a new entry.";
    public const string AlreadyExists = "Please choose a different name or update the existing entry if possible.";
    public const string InvalidOperationAction = "Ensure the operation is allowed. Refer to documentation or seek support if necessary.";
    public const string MissingValue = "Please fill in all required fields to proceed.";
    #endregion

    #region error title 
    public const string ModelingExisted = "ERROR_ModelingExisted";
    public const string ModelingRequiredDataNull = "ERROR_ModelingRequiredDataNull";
    public const string NotificationSettingExisted = "ERROR_NotificationSettingExisted";
    public const string InvalidOperation = "ERROR_InvalidOperation";
    public const string EntityNotExisted = "ERROR_EntityNotExisted";
    public const string EntityExisted = "ERROR_EntityExisted";
    public const string NameExisted = "ERROR_NameExisted";
    public const string DisplayNameExisted = "ERROR_DisplayNameExisted";
    #endregion
}
