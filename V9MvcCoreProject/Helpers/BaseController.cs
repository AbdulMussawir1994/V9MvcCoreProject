using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.Json;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;

namespace V9MvcCoreProject.Helpers;

public class BaseController : Controller
{
    public IActivityHistory _ActivityLog;
    public BaseController(IActivityHistory ActivityLog)
    {
        _ActivityLog = ActivityLog;
    }

    public int GetLoggedInUserId()
    {
        var principal = HttpContext?.User;

        if (principal?.Identity?.IsAuthenticated != true)
            return 0;

        return int.TryParse(principal.FindFirst("UserId")?.Value, out var userId)
            ? userId
            : 0;
    }

    //public string? GetLoggedInUserId()
    //{
    //    var principal = HttpContext?.User;

    //    if (principal?.Identity?.IsAuthenticated != true)
    //        return null;

    //    // Single-pass search, no LINQ allocations
    //    return principal.FindFirst("UserId")?.Value;
    //}


    //public bool ReadOnlyMode(SetUpForm setUpForm, MakerAction action, int referenceId)
    //{
    //    bool lockRecord = false;

    //    lockRecord = Task.Run(async () => await _IMakerCheckerService.LockRecord(setUpForm, action, referenceId)).Result;


    //    return lockRecord;

    //    //return false;
    //}
    //public bool OnBoardingReadOnlyMode(SetUpForm setUpForm, int referenceId)
    //{
    //    bool lockRecord = false;

    //    lockRecord = Task.Run(async () => await _IMakerCheckerService.LockOnBoardingRecord(setUpForm, referenceId)).Result;


    //    return lockRecord;

    //    //return false;
    //}

    public string GetRecordStatus(int stateId)
    {
        return stateId switch
        {
            1 => "Change Request",

            2 => "Remove Request",

            _ => string.Empty,
        };
    }
    public async Task<bool> InsertActivityInsert(object Oldvalue, object NewValue, string Action, [CallerMemberName] string caller = null)
    {
        bool status = true;
        UserHistoryLogs history = new UserHistoryLogs();
        history.OldValueJson = Oldvalue != null ? JsonSerializer.Serialize(Oldvalue) : "";
        history.NewValueJson = NewValue != null ? JsonSerializer.Serialize(NewValue) : "";
        history.Action = Action;
        history.ActionMethod = caller;
        history.UserId = this.GetLoggedInUserId();
        history.LogId = HttpContext.Session.GetString("logId");
        status = await _ActivityLog.SaveActivityLogs(history);
        return status;
    }

}
