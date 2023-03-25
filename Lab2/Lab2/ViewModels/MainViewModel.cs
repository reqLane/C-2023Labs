using Lab2.Exceptions;
using Lab2.Managers;
using Lab2.Models;
using Lab2.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Lab2.ViewModels
{
    class MainViewModel : INotifyPropertyChanged, ILoaderOwner
    {
        #region Fields
        private readonly Person _person = new("", "", "", DateTime.Now);
        private bool _isEnabled = true;
        private Visibility _loaderVisibility = Visibility.Collapsed;
        private RelayComand<object>? _proceedCommand;
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Properties
        public string Name { 
            get { return _person.Name; }
            set { _person.Name = value; } 
        }
        public string Surname
        {
            get { return _person.Surname; }
            set { _person.Surname = value; }
        }
        public string Email
        {
            get { return _person.Email; }
            set { _person.Email = value; }
        }
        public DateTime BirthDate
        {
            get { return _person.BirthDate; }
            set { _person.BirthDate = value; }
        }
        public string? BirthDateText { get; set; }

        public bool? IsAdult { get; private set; }
        public string? SunSign { get; private set; }
        public string? ChineseSign { get; private set; }
        public bool? IsBirthday { get; private set; }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public RelayComand<object> ProceedCommand
        {
            get
            {
                return _proceedCommand ??= new RelayComand<object>(_ => Proceed(), CanProceed);
            }
        }
        #endregion

        private void CalculateInfo()
        {
            //Birth date check
            try
            {
                TryCheckIfCorrect(BirthDate);
            }
            catch (BirthDateException)
            {
                throw;
            }
            //Email check
            if (!Regex.IsMatch(Email, "\\S+@\\S+\\.\\S+"))
            {
                throw new IncorrectEmailException(Email);
            }

            CheckAdult();
            SetSunSign();
            SetChineseSign();
            CheckBirthday();
            //for (int i = 0; i < 1000000000; i++) { }
            ChangeProperties();
        }
        private void CheckAdult()
        {
            DateTime now = DateTime.Now;
            DateTime adult = new(now.Year - 18, now.Month, now.Day);

            IsAdult = DateTime.Compare(BirthDate, adult) < 0;
        }
        private void SetSunSign()
        {
            int day = BirthDate.Day;
            int month = BirthDate.Month;
            if ((day >= 21 && month == 1) || (day <= 19 && month == 2))
                SunSign = "Aquarius";
            else if ((day >= 20 && month == 2) || (day <= 20 && month == 3))
                SunSign = "Pisces";
            else if ((day >= 21 && month == 3) || (day <= 20 && month == 4))
                SunSign = "Aries";
            else if ((day >= 21 && month == 4) || (day <= 21 && month == 5))
                SunSign = "Taurus";
            else if ((day >= 22 && month == 5) || (day <= 21 && month == 6))
                SunSign = "Gemini";
            else if ((day >= 21 && month == 6) || (day <= 23 && month == 7))
                SunSign = "Cancer";
            else if ((day >= 24 && month == 7) || (day <= 23 && month == 8))
                SunSign = "Leo";
            else if ((day >= 24 && month == 8) || (day <= 23 && month == 9))
                SunSign = "Virgo";
            else if ((day >= 24 && month == 9) || (day <= 23 && month == 10))
                SunSign = "Libra";
            else if ((day >= 24 && month == 10) || (day <= 22 && month == 11))
                SunSign = "Scorpio";
            else if ((day >= 23 && month == 11) || (day <= 21 && month == 12))
                SunSign = "Sagittarius";
            else if ((day >= 22 && month == 12) || (day <= 20 && month == 1))
                SunSign = "Capricorn";
        }
        private void SetChineseSign()
        {
            switch (BirthDate.Year % 12)
            {
                case 0:
                    ChineseSign = "Monkey";
                    break;
                case 1:
                    ChineseSign = "Rooster";
                    break;
                case 2:
                    ChineseSign = "Dog";
                    break;
                case 3:
                    ChineseSign = "Pig";
                    break;
                case 4:
                    ChineseSign = "Rat";
                    break;
                case 5:
                    ChineseSign = "Ox";
                    break;
                case 6:
                    ChineseSign = "Tiger";
                    break;
                case 7:
                    ChineseSign = "Rabbit";
                    break;
                case 8:
                    ChineseSign = "Dragon";
                    break;
                case 9:
                    ChineseSign = "Snake";
                    break;
                case 10:
                    ChineseSign = "Horse";
                    break;
                case 11:
                    ChineseSign = "Goat";
                    break;
            }
        }
        private void CheckBirthday()
        {
            IsBirthday = (DateTime.Now.Month == BirthDate.Month && DateTime.Now.Day == BirthDate.Day);
        }
        private void ChangeProperties()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Surname));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(BirthDateText));
            OnPropertyChanged(nameof(IsAdult));
            OnPropertyChanged(nameof(SunSign));
            OnPropertyChanged(nameof(ChineseSign));
            OnPropertyChanged(nameof(IsBirthday));
        }

        private static void TryCheckIfCorrect(DateTime date)
        {
            DateTime now = DateTime.Now;
            DateTime tooOld = new(now.Year - 135, now.Month, now.Day);

            //Future date check
            if (DateTime.Compare(date, now) > 0)
            {
                throw new FutureBirthDateException();
            }
            //Very old date check
            if (DateTime.Compare(tooOld, date) > 0)
            {
                throw new TooOldBirthDateException();
            }
        }

        private async void Proceed()
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                await Task.Run(() => CalculateInfo());
            }
            catch (IncorrectInputException e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }

            if (DateTime.Now.Month == BirthDate.Month && DateTime.Now.Day == BirthDate.Day)
            {
                MessageBox.Show("Happy Birthday!");
            }
        }
        private bool CanProceed(object obj)
        {
            DateTime toParse;
            return !String.IsNullOrWhiteSpace(Name)
                && !String.IsNullOrWhiteSpace(Surname)
                && !String.IsNullOrWhiteSpace(Email)
                && DateTime.TryParse(BirthDateText, out toParse);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
