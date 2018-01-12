using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using BCMS.BusinessLogic;
using BCMS.BusinessLogic.Models;
using BCMS.BusinessLogic.Services;
using BCMS.WebApp.Filters;
using BCMS.WebApp.Helper;
using log4net;

namespace BCMS.WebApp.Controllers
{
    [Authorize]
    public class LeaveController : Controller
    {
        private ILog _log = LogManager.GetLogger(typeof(LeaveController));

        [BcmsAuthenticationActionFilter(ModuleRoleConstants.Module_LeaveRequest, true, false)]
        public ActionResult Index()
        {
            ViewBag.Title = "Leave Request Approval";
            var model = new List<LeaveApplicationObj>();
            return View("Index", model);
        }


        [BcmsAuthenticationActionFilter(ModuleRoleConstants.Module_LeaveRequest, true, true)]
        public ActionResult Approve(int ID)
        {
            var actionCodes = RoleModuleMappingService.Instance.GetActionCodesOfModule(User.Identity.Name,
             ModuleRoleConstants.Module_LeaveRequest);
            var hasViewAccess = actionCodes.Data.Contains(ModuleRoleConstants.ActionView);
            var hasUpdateAccess = actionCodes.Data.Contains(ModuleRoleConstants.ActionUpdate);
            if (!hasViewAccess || !hasUpdateAccess)
            {
                this.ShowMessage(false, "", ModuleRoleConstants.NoAccessErrorMsg);
                return View("Index");
            }



            ViewBag.Title = "Leave Approval";
            var model = new List<LeaveApplicationObj>();
            bool isSuccess = false;

            try
            {
                var result = LeaveService.Instance.ApproveLeaveApplication(ID);
                if (result.IsSuccess)
                {
                    isSuccess = true;
                    ViewBag.IsShowSuccessfullMessage = true;
                    ViewBag.Message = string.Format("Leave application {0} to {1} has been approved.",
                        result.Data.StartDate.ToString("yyyy-MM-dd"),
                        result.Data.EndDate.ToString("yyyy-MM-dd"));

                    var leaveRecord = LeaveService.Instance.GetLeaveApplication(ID);
                    if (leaveRecord.IsSuccess)
                    {
                        var broadCastRecord = BroadcastService.Instance.GetBusCaptainGCMID(leaveRecord.Data.PersonnelNumber);

                            var err = GoogleNotification.CallGoogleAPI(broadCastRecord.Data.GcmID, "Leave Approval",
                                ViewBag.Message);

                            var broadcast = new BroadcastController();
                            bool apnsErr = AppleNotification.Push(broadCastRecord.Data.GcmID, ViewBag.Message);
                        
                        if (err != "")
                        {
                            _log.ErrorFormat("error while doing google notification on leave approval");
                        }
                    }
                    else
                    {
                        ViewBag.IsShowErrorMessage = true;
                        ViewBag.Message = leaveRecord.ErrorInfo;
                    }
                }
                else
                {
                    ViewBag.IsShowErrorMessage = true;
                    ViewBag.Message = result.ErrorInfo;
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsShowErrorMessage = true;
                ViewBag.Message = this.CleanJavaScriptString(ex.ToString());
                _log.Error(ex);
            }

            BusCaptainService.Instance.saveActivity(Activities.LeaveApprove.ToString(), User.Identity.Name,
                "", "", isSuccess, ID);

            return View("Index", model);
        }

        [BcmsAuthenticationActionFilter(ModuleRoleConstants.Module_LeaveRequest, true, true)]
        public ActionResult Reject(int ID)
        {
            ViewBag.Title = "Leave Reject";
            var model = new List<LeaveApplicationObj>();
            bool isSuccess = false;

            try
            {
                var result = LeaveService.Instance.RejectLeaveApplication(ID);
                if (result.IsSuccess)
                {
                    isSuccess = true;
                    ViewBag.IsShowSuccessfullMessage = true;
                    ViewBag.Message = string.Format("Leave application {0} to {1} has been rejected.",
                        result.Data.StartDate.ToString("yyyy-MM-dd"),
                        result.Data.EndDate.ToString("yyyy-MM-dd"));

                    var leaveRecord = LeaveService.Instance.GetLeaveApplication(ID);
                    if (leaveRecord.IsSuccess)
                    {
                        var broadCastRecord = BroadcastService.Instance.GetBusCaptainGCMID(leaveRecord.Data.PersonnelNumber);
                        var err = GoogleNotification.CallGoogleAPI(broadCastRecord.Data.GcmID, "Leave Rejected", ViewBag.Message);
                        if (err != "")
                        {
                            _log.ErrorFormat("error while doing google notification on leave reject");
                        }
                    }
                    else
                    {
                        ViewBag.IsShowSuccessfullMessage = true;
                        ViewBag.Message = leaveRecord.ErrorInfo;
                    }
                }
                else
                {
                    ViewBag.IsShowSuccessfullMessage = true;
                    ViewBag.Message = result.ErrorInfo;
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsShowErrorMessage = true;
                ViewBag.Message = this.CleanJavaScriptString(ex.ToString());
                _log.Error(ex);
            }

            BusCaptainService.Instance.saveActivity(Activities.LeaveReject.ToString(), User.Identity.Name,
               "", "", isSuccess, ID);

            return View("Index", model);
        }


        [BcmsAuthenticationActionFilter(ModuleRoleConstants.Module_LeaveRequest, true, false)]
        public ActionResult Detail(int id)
        {
            var result = LeaveService.Instance.GetLeaveApplication(id);
            if (result.IsSuccess) { return View("LeaveDetails", result.Data); }
            else
            {
                ViewBag.IsShowErrorMessage = true;
                ViewBag.Message = result.ErrorInfo;
                return View("index", new List<LeaveApplicationObj>());
            }
        }

        #region DataTables - JSON 

        public ActionResult GetJsonData(int draw, int start, int length)
        {
            string search = Request.QueryString[DataTableQueryString.Searching];
            string sortColumn = "";
            string sortDirection = "asc";

            if (Request.QueryString[DataTableQueryString.OrderingColumn] != null)
            {
                sortColumn = GetServiceSortingColumn(Request.QueryString[DataTableQueryString.OrderingColumn].ToString());
            }
            if (Request.QueryString[DataTableQueryString.OrderingDir] != null)
            {
                sortDirection = Request.QueryString[DataTableQueryString.OrderingDir];
            }

            var dataTableData = new DataTableData
            {
                draw = draw
            };
            var recordsFiltered = 0;
            var result = LeaveService.Instance.GetLeaveApplicationsForSupervisor(User.Identity.Name, out recordsFiltered, start, length, sortColumn, sortDirection, search);

            if (result.IsSuccess)
            {
                dataTableData.data = result.Data;
            }

            dataTableData.recordsFiltered = recordsFiltered;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }


        public string GetServiceSortingColumn(string sortColumnNo)
        {
            var name = DataTableHelper.SoringColumnName<LeaveApplicationObj>(sortColumnNo);
            return name;
        }

        public class DataTableData
        {
            public int draw { get; set; }
            public int recordsFiltered { get; set; }
            public List<LeaveApplicationObj> data { get; set; }
        }
        #endregion
    }

    public class LeaveIndexViewModel
    {
        public bool IsShowSuccessfullMessage = false;

        public bool IsShowErrorMessage = false;

        public string Message = "";

        public List<LeaveApplicationObj> Applications = new List<LeaveApplicationObj>();
    }

}
