namespace prepo.Api.Models
{
    /*
    public class SaveResourceResult<T>
    {
        private readonly ActionPerfomedOptions _actionPerfomed;

        public SaveResourceResult(ActionPerfomedOptions actionPerfomed)
        {
            _actionPerfomed = actionPerfomed;
        }

        // public string Location { get; set; }

        public ActionPerfomedOptions ActionPerfomed
        {
            get { return _actionPerfomed; }
        }

        // public T Instance { get; set; }
    }
    */

    public enum ActionPerfomed
    {
        NotSuported,
        Deleted,
        Created,
        Updated,
    }
}