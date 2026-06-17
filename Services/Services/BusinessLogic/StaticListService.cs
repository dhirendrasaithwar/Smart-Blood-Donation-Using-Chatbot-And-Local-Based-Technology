using Repository;
using System.Web.WebPages.Html;

namespace Services
{
    public class StaticListService: IStaticListService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaticListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SelectListItem> BloodTypes()
        {
            List<SelectListItem> bloodType = new List<SelectListItem>();
            var bloodList = _unitOfWork._db.BloodTypes.ToList();

            bloodType.Add(new SelectListItem() { Text = "--Select Blood Type--", Value = "", Selected = true });

            foreach (var row in bloodList)
            {
                bloodType.Add(new SelectListItem() { Text = row.BloodTypes, Value = row.BloodTypeId.ToString() });
            }

            return bloodType;
        }

        public List<SelectListItem> QuestionCategory()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem(){Text = "--Select Question Category--", Value = "", Selected = true});
            list.Add(new SelectListItem() { Text = "--Select Question Category--", Value = "", Selected = true });
            list.Add(new SelectListItem() { Text = "Eligibility", Value = "Eligibility" });
            list.Add(new SelectListItem() { Text = "Donation Process", Value = "Donation Process" });
            list.Add(new SelectListItem() { Text = "Blood Types", Value = "Blood Types" });
            list.Add(new SelectListItem() { Text = "Health & Safety", Value = "Health & Safety" });
            list.Add(new SelectListItem() { Text = "System & Appointments", Value = "System & Appointments" });
            list.Add(new SelectListItem() { Text = "Motivation & Impact", Value = "Motivation & Impact" });
            list.Add(new SelectListItem() { Text = "Blood Stock & Inventory", Value = "Blood Stock & Inventory" });
            
            return list;
        }

        public List<SelectListItem> BloodRequestStatus()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem(){Text = "--Select Blood Request Status--", Value = "", Selected = true});
            list.Add(new SelectListItem(){Text = "Pending", Value = "Pending", Selected = true});
            list.Add(new SelectListItem(){Text = "Approved", Value = "Approved", Selected = true});
            list.Add(new SelectListItem(){Text = "Expired", Value = "Expired", Selected = true});
            
            return list;
        }
    }
    public interface IStaticListService
    {
        List<SelectListItem> BloodTypes();
        List<SelectListItem> QuestionCategory();
    }
}
