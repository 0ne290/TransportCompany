using Microsoft.AspNetCore.Mvc;

namespace Web.ActionResults;

public static class ActionResultFactory
{
    public static ViewResult CustomServerErrorView(Controller controller)
    {
        var errorView = controller.View("CustomError");
        errorView.StatusCode = 500;
        return errorView;
    }
}