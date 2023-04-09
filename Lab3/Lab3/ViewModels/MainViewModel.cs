using Lab3.Exceptions;
using Lab3.Managers;
using Lab3.Models;
using Lab3.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3.ViewModels
{
    class MainViewModel : INotifyPropertyChanged, ILoaderOwner
    {
        #region Fields
        private ObservableCollection<Person> _persons;
        private readonly Person _personToAdd = new("", "", "", DateTime.Now);
        private readonly Person _personToEdit = new("", "", "", DateTime.Now);
        private string _emailToDelete = "";
        private bool _isEnabled = true;
        private Visibility _loaderVisibility = Visibility.Collapsed;
        private RelayComand<object>? _addUserCommand;
        private RelayComand<object>? _editUserCommand;
        private RelayComand<object>? _deleteUserCommand;
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Properties
        public string NameAdd { 
            get { return _personToAdd.Name; }
            set { _personToAdd.Name = value; } 
        }
        public string SurnameAdd
        {
            get { return _personToAdd.Surname; }
            set { _personToAdd.Surname = value; }
        }
        public string EmailAdd
        {
            get { return _personToAdd.Email; }
            set { _personToAdd.Email = value; }
        }
        public DateTime BirthDateAdd
        {
            get { return _personToAdd.BirthDate; }
            set { _personToAdd.BirthDate = value; }
        }
        public string? BirthDateAddText { get; set; }

        public string NameEdit
        {
            get { return _personToEdit.Name; }
            set { _personToEdit.Name = value; }
        }
        public string SurnameEdit
        {
            get { return _personToEdit.Surname; }
            set { _personToEdit.Surname = value; }
        }
        public string EmailEdit
        {
            get { return _personToEdit.Email; }
            set { _personToEdit.Email = value; }
        }
        public DateTime BirthDateEdit
        {
            get { return _personToEdit.BirthDate; }
            set { _personToEdit.BirthDate = value; }
        }
        public string? BirthDateEditText { get; set; }

        public string EmailDelete
        {
            get { return _emailToDelete; }
            set { _emailToDelete = value; }
        }

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
        public RelayComand<object> AddUserCommand
        {
            get
            {
                return _addUserCommand ??= new RelayComand<object>(_ => AddUser(), CanAddUser);
            }
        }
        public RelayComand<object> EditUserCommand
        {
            get
            {
                return _editUserCommand ??= new RelayComand<object>(_ => EditUser(), CanEditUser);
            }
        }
        public RelayComand<object> DeleteUserCommand
        {
            get
            {
                return _deleteUserCommand ??= new RelayComand<object>(_ => DeleteUser(), CanDeleteUser);
            }
        }
        #endregion

        private void CalculateInfo(Person person)
        {
            //Birth date check
            try
            {
                TryCheckIfCorrect(person.BirthDate);
            }
            catch (BirthDateException)
            {
                throw;
            }
            //Email check
            if (!Regex.IsMatch(person.Email, "\\S+@\\S+\\.\\S+"))
            {
                throw new IncorrectEmailException(person.Email);
            }

            CheckAdult(person);
            SetSunSign(person);
            SetChineseSign(person);
            CheckBirthday(person);
            ChangeProperties();
        }
        private void CheckAdult(Person person)
        {
            DateTime now = DateTime.Now;
            DateTime adult = new(now.Year - 18, now.Month, now.Day);

            person.IsAdult = DateTime.Compare(person.BirthDate, adult) < 0;
        }
        private void SetSunSign(Person person)
        {
            int day = person.BirthDate.Day;
            int month = person.BirthDate.Month;
            if ((day >= 21 && month == 1) || (day <= 19 && month == 2))
                person.SunSign = "Aquarius";
            else if ((day >= 20 && month == 2) || (day <= 20 && month == 3))
                person.SunSign = "Pisces";
            else if ((day >= 21 && month == 3) || (day <= 20 && month == 4))
                person.SunSign = "Aries";
            else if ((day >= 21 && month == 4) || (day <= 21 && month == 5))
                person.SunSign = "Taurus";
            else if ((day >= 22 && month == 5) || (day <= 21 && month == 6))
                person.SunSign = "Gemini";
            else if ((day >= 21 && month == 6) || (day <= 23 && month == 7))
                person.SunSign = "Cancer";
            else if ((day >= 24 && month == 7) || (day <= 23 && month == 8))
                person.SunSign = "Leo";
            else if ((day >= 24 && month == 8) || (day <= 23 && month == 9))
                person.SunSign = "Virgo";
            else if ((day >= 24 && month == 9) || (day <= 23 && month == 10))
                person.SunSign = "Libra";
            else if ((day >= 24 && month == 10) || (day <= 22 && month == 11))
                person.SunSign = "Scorpio";
            else if ((day >= 23 && month == 11) || (day <= 21 && month == 12))
                person.SunSign = "Sagittarius";
            else if ((day >= 22 && month == 12) || (day <= 20 && month == 1))
                person.SunSign = "Capricorn";
        }
        private void SetChineseSign(Person person)
        {
            switch (person.BirthDate.Year % 12)
            {
                case 0:
                    person.ChineseSign = "Monkey";
                    break;
                case 1:
                    person.ChineseSign = "Rooster";
                    break;
                case 2:
                    person.ChineseSign = "Dog";
                    break;
                case 3:
                    person.ChineseSign = "Pig";
                    break;
                case 4:
                    person.ChineseSign = "Rat";
                    break;
                case 5:
                    person.ChineseSign = "Ox";
                    break;
                case 6:
                    person.ChineseSign = "Tiger";
                    break;
                case 7:
                    person.ChineseSign = "Rabbit";
                    break;
                case 8:
                    person.ChineseSign = "Dragon";
                    break;
                case 9:
                    person.ChineseSign = "Snake";
                    break;
                case 10:
                    person.ChineseSign = "Horse";
                    break;
                case 11:
                    person.ChineseSign = "Goat";
                    break;
            }
        }
        private void CheckBirthday(Person person)
        {
            person.IsBirthday = (DateTime.Now.Month == person.BirthDate.Month && DateTime.Now.Day == person.BirthDate.Day);
        }
        private void ChangeProperties()
        {
            //OnPropertyChanged(nameof(Name));
            //OnPropertyChanged(nameof(Surname));
            //OnPropertyChanged(nameof(Email));
            //OnPropertyChanged(nameof(BirthDateText));
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

        private async void AddUser()
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                await Task.Run(() => CalculateInfo(_personToAdd));
            }
            catch (IncorrectInputException e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }

            if (DateTime.Now.Month == BirthDateAdd.Month && DateTime.Now.Day == BirthDateAdd.Day)
            {
                MessageBox.Show("Happy Birthday!");
            }
        }
        private bool CanAddUser(object obj)
        {
            DateTime toParse;
            return !String.IsNullOrWhiteSpace(NameAdd)
                && !String.IsNullOrWhiteSpace(SurnameAdd)
                && !String.IsNullOrWhiteSpace(EmailAdd)
                && DateTime.TryParse(BirthDateAddText, out toParse);
        }

        private async void EditUser()
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                await Task.Run(() => CalculateInfo(_personToEdit));
            }
            catch (IncorrectInputException e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }

            if (DateTime.Now.Month == BirthDateAdd.Month && DateTime.Now.Day == BirthDateAdd.Day)
            {
                MessageBox.Show("Happy Birthday!");
            }
        }
        private bool CanEditUser(object obj)
        {
            DateTime toParse;
            return !String.IsNullOrWhiteSpace(NameEdit)
                && !String.IsNullOrWhiteSpace(SurnameEdit)
                && !String.IsNullOrWhiteSpace(EmailEdit)
                && DateTime.TryParse(BirthDateEditText, out toParse);
        }

        private async void DeleteUser()
        {

        }
        private bool CanDeleteUser(object obj)
        {
            return !String.IsNullOrWhiteSpace(EmailDelete);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
