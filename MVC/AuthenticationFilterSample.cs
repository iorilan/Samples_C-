    public class BcmsAuthenticationActionFilter : ActionFilterAttribute
    {
        private readonly string _moduleName;
        private readonly bool _viewAccessCheck;
        private readonly bool _updateAccessCheck;
        private bool hasAccessToModule = true;
        public BcmsAuthenticationActionFilter(
            string moduleName, bool checkViewAccess, bool checkUpdateAccess)
        {
            _moduleName = moduleName;
            _viewAccessCheck = checkViewAccess;
            _updateAccessCheck = checkUpdateAccess;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            string username = HttpContext.Current.User.Identity.Name;
            var isSuperAdmin = HttpContext.Current.User.IsInRole(ModuleRoleConstants.RoleNames.SuperAdmin);
            if (isSuperAdmin)
            {
                return;
            }


            var actionCodes = RoleModuleMappingService.Instance.GetActionCodesOfModule(username,
              _moduleName);
            

            if (!actionCodes.IsSuccess)
            {
                hasAccessToModule = false;

                filterContext.Result = new ContentResult()
                {
                    Content = ModuleRoleConstants.NoAccessErrorMsg
                };
                return;
            }


            if (_viewAccessCheck &&
                !actionCodes.Data.Contains(ModuleRoleConstants.ActionView))
            {
                hasAccessToModule = false;
            }
            if (_updateAccessCheck &&
                !actionCodes.Data.Contains(ModuleRoleConstants.ActionUpdate))
            {
                hasAccessToModule = false;
            }

            if (!hasAccessToModule)
            {
                filterContext.Controller.TempData["IsShowErrorMessage"] = true;
                filterContext.Controller.TempData["Message"] = ModuleRoleConstants.NoAccessErrorMsg;

                filterContext.Result = new ContentResult()
                {
                    Content = ModuleRoleConstants.NoAccessErrorMsg
                };
            }

        }


    }