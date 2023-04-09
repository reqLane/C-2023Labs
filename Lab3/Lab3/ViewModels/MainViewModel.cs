using Lab3.Exceptions;
using Lab3.Managers;
using Lab3.Models;
using Lab3.Repositories;
using Lab3.Tools;
using System;
using System.Collections;
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
        private List<string> _emails = new List<string>();
        private List<Person> _persons;
        private ObservableCollection<Person> _personsDisplayed;
        private FileRepository _fileRepository = new FileRepository();
        private readonly Person _personToAdd = new("", "", "", DateTime.Now);
        private string _emailToDelete = "";
        private bool _isEnabled = true;
        private Visibility _loaderVisibility = Visibility.Collapsed;
        private RelayComand<object>? _addUserCommand;
        private RelayComand<object>? _deleteUserCommand;
        private RelayComand<object>? _filterPersonsCommand;
        private RelayComand<object>? _sortNameCommand;
        private RelayComand<object>? _sortSurnameCommand;
        private RelayComand<object>? _sortEmailCommand;
        private RelayComand<object>? _sortBirthDateCommand;
        private RelayComand<object>? _sortIsAdultCommand;
        private RelayComand<object>? _sortSunSignCommand;
        private RelayComand<object>? _sortChineseSignCommand;
        private RelayComand<object>? _sortIsBirthdayCommand;
        private RelayComand<object>? _saveConfigCommand;
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        public MainViewModel()
        {
            _persons = _fileRepository.GetAllPersons();
            foreach (var person in _persons)
            {
                _emails.Add(person.Email);
            }
            _personsDisplayed = new ObservableCollection<Person>(_persons);
            NameFilter = "";
            SurnameFilter = "";
            EmailFilter = "";
            BirthDateFilter = DateTime.MinValue;
            IsAdultFilter = "";
            SunSignFilter = "";
            ChineseSignFilter = "";
            IsBirthdayFilter = "";
        }

        #region Properties
        public ObservableCollection<Person> PersonsDisplayed
        {
            get { return _personsDisplayed; }
            set
            {
                _personsDisplayed = value;
                OnPropertyChanged();
            }
        }

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

        public string EmailDelete
        {
            get { return _emailToDelete; }
            set { _emailToDelete = value; }
        }

        public string NameFilter { get; set; }
        public string SurnameFilter { get; set; }
        public string EmailFilter { get; set; }
        public DateTime BirthDateFilter { get; set; }
        public string IsAdultFilter { get; set; }
        public string SunSignFilter { get; set; }
        public string ChineseSignFilter { get; set; }
        public string IsBirthdayFilter { get; set; }

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
        public RelayComand<object> DeleteUserCommand
        {
            get
            {
                return _deleteUserCommand ??= new RelayComand<object>(_ => DeleteUser(), CanDeleteUser);
            }
        }
        public RelayComand<object> FilterPersonsCommand
        {
            get
            {
                return _filterPersonsCommand ??= new RelayComand<object>(_ => FilterPersons(), CanFilterPersons);
            }
        }
        public RelayComand<object> SortNameCommand
        {
            get
            {
                return _sortNameCommand ??= new RelayComand<object>(_ => SortPersons("Name"), CanSortPersons);
            }
        }
        public RelayComand<object> SortSurnameCommand
        {
            get
            {
                return _sortSurnameCommand ??= new RelayComand<object>(_ => SortPersons("Surname"), CanSortPersons);
            }
        }
        public RelayComand<object> SortEmailCommand
        {
            get
            {
                return _sortEmailCommand ??= new RelayComand<object>(_ => SortPersons("Email"), CanSortPersons);
            }
        }
        public RelayComand<object> SortBirthDateCommand
        {
            get
            {
                return _sortBirthDateCommand ??= new RelayComand<object>(_ => SortPersons("BirthDate"), CanSortPersons);
            }
        }
        public RelayComand<object> SortIsAdultCommand
        {
            get
            {
                return _sortIsAdultCommand ??= new RelayComand<object>(_ => SortPersons("IsAdult"), CanSortPersons);
            }
        }
        public RelayComand<object> SortSunSignCommand
        {
            get
            {
                return _sortSunSignCommand ??= new RelayComand<object>(_ => SortPersons("SunSign"), CanSortPersons);
            }
        }
        public RelayComand<object> SortChineseSignCommand
        {
            get
            {
                return _sortChineseSignCommand ??= new RelayComand<object>(_ => SortPersons("ChineseSign"), CanSortPersons);
            }
        }
        public RelayComand<object> SortIsBirthdayCommand
        {
            get
            {
                return _sortIsBirthdayCommand ??= new RelayComand<object>(_ => SortPersons("IsBirthday"), CanSortPersons);
            }
        }
        public RelayComand<object> SaveConfigCommand
        {
            get
            {
                return _saveConfigCommand ??= new RelayComand<object>(_ => SaveConfig());
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
                if (_emails.Contains(EmailAdd))
                    throw new Exception("Person already exists");
                await Task.Run(() => CalculateInfo(_personToAdd));
                _persons.Add(new Person(_personToAdd));
                _emails.Add(EmailAdd);
                PersonsDisplayed = new ObservableCollection<Person>(_persons);
            }
            catch (Exception e)
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

        private async void DeleteUser()
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                if (!_emails.Contains(EmailDelete))
                    throw new Exception("Person not found");
                await Task.Run(() =>
                {
                    for (int i = 0; i < _persons.Count; i++)
                    {
                        Person person = _persons[i];
                        if (person.Email == _emailToDelete)
                        {
                            _persons.RemoveAt(i);
                            _emails.Remove(EmailDelete);
                            break;
                        }
                    }
                });
                PersonsDisplayed = new ObservableCollection<Person>(_persons);
            }
            catch (Exception e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }
        }
        private bool CanDeleteUser(object obj)
        {
            return !String.IsNullOrWhiteSpace(EmailDelete);
        }

        private async void FilterPersons()
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                await Task.Run(() =>
                {
                    var filteredPersons = from p in _persons
                                          where p.Name.Contains(NameFilter)
                                          && p.Surname.Contains(SurnameFilter)
                                          && p.Email.Contains(EmailFilter)
                                          && (BirthDateFilter == DateTime.MinValue || p.BirthDate.Equals(BirthDateFilter))
                                          && (String.IsNullOrWhiteSpace(IsAdultFilter) || p.IsAdult.Equals(IsAdultFilter == "true" ? true : false))
                                          && p.SunSign != null && p.ChineseSign != null
                                          && p.SunSign.Contains(SunSignFilter)
                                          && p.ChineseSign.Contains(ChineseSignFilter)
                                          && (String.IsNullOrWhiteSpace(IsBirthdayFilter) || p.IsBirthday.Equals(IsBirthdayFilter == "true" ? true : false))
                                          select p;

                    PersonsDisplayed = new ObservableCollection<Person>(filteredPersons.ToList());
                });
            }
            catch (Exception e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }
        }
        private bool CanFilterPersons(object obj)
        {
            return (String.IsNullOrWhiteSpace(IsAdultFilter) || IsAdultFilter == "true" || IsAdultFilter == "false")
                && (String.IsNullOrWhiteSpace(IsBirthdayFilter) || IsBirthdayFilter == "true" || IsBirthdayFilter == "false");
        }

        private async void SortPersons(string field)
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                await Task.Run(() =>
                {
                    IEnumerable<Person>? sortedPersons = null;
                    switch (field)
                    {
                        case "Name":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.Name);
                            break;
                        case "Surname":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.Surname);
                            break;
                        case "Email":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.Email);
                            break;
                        case "BirthDate":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.BirthDate);
                            break;
                        case "IsAdult":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.IsAdult);
                            break;
                        case "SunSign":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.SunSign);
                            break;
                        case "ChineseSign":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.ChineseSign);
                            break;
                        case "IsBirthday":
                            sortedPersons = _personsDisplayed.OrderBy(p => p.IsBirthday);
                            break;
                    }

                    if(sortedPersons != null)
                        PersonsDisplayed = new ObservableCollection<Person>(sortedPersons.ToList());
                });
            }
            catch (Exception e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }
        }
        private bool CanSortPersons(object obj)
        {
            return PersonsDisplayed.Count > 1;
        }

        private async void SaveConfig()
        {
            try
            {
                LoaderManager.Instance.ShowLoader();
                await _fileRepository.RewriteConfig(_persons);
            }
            catch (Exception e)
            {
                LoaderManager.Instance.HideLoader();
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
