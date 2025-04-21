namespace nScheduler.Web;

public class HttpInterfaces
{
    public static string Index => "api/Index";
    public static string ParamList => "/api/ParameterCfg/List";

    public static string ParamPage => "/api/ParameterCfg/Page";

    public static string ParamEdit => "/api/ParameterCfg/Edit";

    public static string ParamRemove => "/api/ParameterCfg/Delete";

    public static string MessagePage => "/api/MessageCfg/Page";

    public static string MessageEdit => "/api/MessageCfg/Edit";

    public static string MessageRemove => "/api/MessageCfg/Delete";

    public static string ImageList => "/api/ImageCfg/List";

    public static string ImagePage => "/api/ImageCfg/Page";

    public static string ImageEdit => "/api/ImageCfg/Edit";

    public static string ImageRemove => "/api/ImageCfg/Delete";

    public static string UserPage => "/api/User/Page";

    public static string UserEdit => "/api/User/Edit";

    public static string UserRemove => "/api/User/Delete";

    public static string JobInfo => "/api/JobInfo/Get";

    public static string JobPage => "/api/JobInfo/Page";

    public static string JobEdit => "/api/JobInfo/Edit";

    public static string JobRemove => "/api/JobInfo/Delete";

    public static string JobExec => "/api/JobInfo/Exec";

    public static string JobInValid => "/api/JobInfo/InValid";

    public static string LogPage => "/api/JobLog/Page";

    public static string LogDetail => "/api/JobLog/Detail";

    public static string LogStop => "/api/JobLog/Stop";
}