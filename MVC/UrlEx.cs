   public static RouteValueDictionary ToRouteValues(this NameValueCollection queryString)
        {
            if (queryString == null || !queryString.HasKeys())
            {
                return new RouteValueDictionary();
            }
            RouteValueDictionary routeValueDictionaries = new RouteValueDictionary();
            string[] allKeys = queryString.AllKeys;
            for (int i = 0; i < (int)allKeys.Length; i++)
            {
                string str = allKeys[i];
                routeValueDictionaries.Add(str, queryString[str]);
            }
            return routeValueDictionaries;
        }

        public static string AddParam(this System.Web.Mvc.UrlHelper helper, string key, object value)
        {
            var routeValueDic = new RouteValueDictionary(new Dictionary<string, object>() {{key, value}});
            return AddParams(helper, routeValueDic);
        }
        public static string AddParams(this System.Web.Mvc.UrlHelper helper, object extra)
        {
            var routeValues = RoutValueHelper.MergeWithCurrentContext(helper.RequestContext, new RouteValueDictionary(extra));
            return AddParams(helper, routeValues);
        }

        public static string AddParams(this System.Web.Mvc.UrlHelper helper, RouteValueDictionary extra)
        {
            var queryString = HttpContext.Current.Request.QueryString.ToRouteValues();
            return helper.Action(helper.RequestContext.RouteData.Values["action"] as string, queryString.Merge(extra));
        }

        public static string ActionwParams(this System.Web.Mvc.UrlHelper helper, string actionName, string controllerName)
        {
            return ActionwParams(helper, actionName, controllerName, null);
        }

        public static string ActionwParams(this System.Web.Mvc.UrlHelper helper, string actionName, string controllerName, RouteValueDictionary extra)
        {
            var r = RoutValueHelper.MergeWithCurrentContext(helper.RequestContext, extra);

            return helper.Action(actionName, controllerName, r);
        }

        public static string ActionArea<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expression) where TController : Controller
        {
            RouteValueDictionary routeValues = GetRouteValuesFromExpression(expression);
            VirtualPathData vpd = urlHelper.RouteCollection.GetVirtualPathForArea(urlHelper.RequestContext, routeValues);
            return (vpd == null) ? null : vpd.VirtualPath;
        }

        public static RouteValueDictionary GetRouteValuesFromExpression<TController>(Expression<Action<TController>> action) where TController : Controller
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            MethodCallExpression call = action.Body as MethodCallExpression;
            if (call == null)
            {
                throw new ArgumentException("Akcja nie może być pusta.", "action");
            }

            string controllerName = typeof(TController).Name;
            if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Docelowa klasa nie jest kontrolerem.(Nie kończy się na 'Controller')", "action");
            }
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            if (controllerName.Length == 0)
            {
                throw new ArgumentException("Nie można przejść do kontrolera.", "action");
            }

            // TODO: How do we know that this method is even web callable?
            //      For now, we just let the call itself throw an exception.

            string actionName = GetTargetActionName(call.Method);

            var rvd = new RouteValueDictionary();
            rvd.Add("Controller", controllerName);
            rvd.Add("Action", actionName);

            var namespaceNazwa = typeof(TController).Namespace;

            if (namespaceNazwa.Contains("Areas."))
            {
                int index = namespaceNazwa.IndexOf('.', namespaceNazwa.IndexOf("Areas."));
                string nazwaArea = namespaceNazwa.Substring(namespaceNazwa.IndexOf("Areas.") + 6, index - namespaceNazwa.IndexOf("Areas.") + 2);
                if (!String.IsNullOrEmpty(nazwaArea))
                {
                    rvd.Add("Area", nazwaArea);
                }
            }

            //var typ = typeof(TController).GetCustomAttributes(typeof(ActionLinkAreaAttribute), true /* inherit */).FirstOrDefault();
            /*ActionLinkAreaAttribute areaAttr = typ as ActionLinkAreaAttribute;
            if (areaAttr != null)
            {
                string areaName = areaAttr.Area;
                rvd.Add("Area", areaName);
            }*/

            AddParameterValuesFromExpressionToDictionary(rvd, call);
            return rvd;
        }


        private static string GetTargetActionName(MethodInfo methodInfo)
        {
            string methodName = methodInfo.Name;

            // do we know this not to be an action?
            if (methodInfo.IsDefined(typeof(NonActionAttribute), true /* inherit */))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                    "Nie można wywoływać metod innych niż akcje.", methodName));
            }

            // has this been renamed?
            ActionNameAttribute nameAttr = methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true /* inherit */).OfType<ActionNameAttribute>().FirstOrDefault();
            if (nameAttr != null)
            {
                return nameAttr.Name;
            }

            // targeting an async action?
            if (methodInfo.DeclaringType.IsSubclassOf(typeof(AsyncController)))
            {
                if (methodName.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
                {
                    return methodName.Substring(0, methodName.Length - "Async".Length);
                }
                if (methodName.EndsWith("Completed", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                       "Nie można wywoływać kompletnych metod.", methodName));
                }
            }

            // fallback
            return methodName;
        }

        static void AddParameterValuesFromExpressionToDictionary(RouteValueDictionary rvd, MethodCallExpression call)
        {
            ParameterInfo[] parameters = call.Method.GetParameters();

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    Expression arg = call.Arguments[i];
                    object value = null;
                    ConstantExpression ce = arg as ConstantExpression;
                    if (ce != null)
                    {
                        // If argument is a constant expression, just get the value
                        value = ce.Value;
                    }
                    else
                    {
                        try
                        {
                            value = CachedExpressionCompiler.Evaluate(arg);
                        }
                        catch (Exception ex)
                        {
                            //TODO log this!!
                            continue;
                        }
                        
                    }
                    rvd.Add(parameters[i].Name, value);
                }
            }
        }