using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddUtil.Models
{
    /// <summary>
    /// Инкапсуляция бизнес логики поздравления.
    /// </summary>
    public class CongratulationModel : ModelBase, IDataErrorInfo
    {
        public CongratulationModel() { }

        public CongratulationModel(CongratulationModel model)
        {
            Id = model.Id;
            Kind = model.Kind;
            Content = model.Content;
            Holiday = model.Holiday;
            Interest = model.Interest;
            Sex = model.Sex;
            Age = model.Age;
        }

        //
        // Поля для определения модели поздравляшки.
        //

        private int id;
        public int Id
        {
            get => id;
            set => this.SetField(ref id, value);
        }

        private string kind;
        public string Kind
        {
            get => kind;
            set => this.SetField(ref kind, value);
        }

        private string content;
        public string Content
        {
            get => content;
            set => this.SetField(ref content, value);
        }

        private string holiday;
        public string Holiday
        {
            get => holiday;
            set => this.SetField(ref holiday, value);
        }

        private string interest;
        public string Interest
        {
            get => interest;
            set => this.SetField(ref interest, value);
        }

        private int sex;
        public int Sex
        {
            get => sex;
            set => this.SetField(ref sex, value);
        }

        private string age;
        public string Age
        {
            get => age;
            set => SetField(ref age, value);
        }

        [NotMapped]
        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Kind":
                        Error = this.GetErrorIfKindEmpty();
                        break;
                    case "Age":
                        Error = this.GetErrorIfAgeIncorrect();
                        break;
                    case "Content":
                        Error = this.GetErrorIfNoContent();
                        break;
                    case "Holiday":
                        Error = this.GetErrorIfHolidayEmpty();
                        break;
                    case "Interest":
                        Error = this.GetErrorIfInterestEmpty();
                        break;
                }

                return Error;
            }
        }

        public bool IsAllFilledCorrect(out List<string> errors)
        {
            errors = new List<string>();

            errors.Add(this.GetErrorIfAgeIncorrect());
            errors.Add(this.GetErrorIfHolidayEmpty());
            errors.Add(this.GetErrorIfInterestEmpty());
            errors.Add(this.GetErrorIfKindEmpty());
            errors.Add(this.GetErrorIfNoContent());

            return (errors.Count == 0);
        }

        private string GetErrorIfInterestEmpty()
        {
            if (this.Interest == null || this.Interest.Length == 0)
                return ("Какие интересы?");
            else
                return string.Empty;
        }

        private string GetErrorIfHolidayEmpty()
        {
            if (this.Holiday == null || this.Content.Length == 0)
                return "К какому празднику относится поздравление?";
            else
                return string.Empty;
        }

        private string GetErrorIfNoContent()
        {
            if (this.Content == null || this.Content.Length == 0)
                return ("Контент должен быть.");
            else
                return string.Empty;
        }

        private string GetErrorIfKindEmpty()
        {
            if (Kind == null || Kind.Length == 0)
                return "Нельзя оставлять вид поздравления пустым.";
            else
                return string.Empty;
        }

        private string GetErrorIfAgeIncorrect()
        {
            if (this.Age == null || this.Age.Length == 0)
                return "Необходим возраст.";


            int age = 0; 
            bool isParsed = int.TryParse(this.Age, out age);

            if (!isParsed)
                return "Должны быть целочисленные значения.";

            if (age >= 120 || age <= 0)
                return "Некорректный возраст. Допустимый дипазон: от 0 до 120 лет.";

            return string.Empty;
        }
    }
}
