namespace prepo.Api.Controllers
{
    public class SaveResourceResult<T>
    {
        private readonly ActionPerfomedOptions _actionPerfomed;

        public SaveResourceResult(ActionPerfomedOptions actionPerfomed)
        {
            _actionPerfomed = actionPerfomed;
        }

        public string Location { get; set; }

        public ActionPerfomedOptions ActionPerfomed
        {
            get { return _actionPerfomed; }
        }

        public enum ActionPerfomedOptions
        {
            Deleted,
            Created,
            Updated,
        }

        public T Resource { get; set; }
    }
}