﻿using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class StoresViewModel : BindableBase
    {
        int _storeId;
        string _name;
        string _motto;
        byte[] _image;
        string _address1;
        string _address2;
        string _tel1;
        string _tel2;
        string _phone1;
        string _phone2;
        string _email1;
        string _email2;
        string _site;
        string _notes;

        public int StoreId
        {
            get => _storeId;
            set
            {
                if (value != _storeId)
                {
                    _storeId = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Motto
        {
            get => _motto;
            set
            {
                if (value != _motto)
                {
                    _motto = value;
                    RaisePropertyChanged();
                }
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Address1
        {
            get => _address1;
            set
            {
                if (value != _address1)
                {
                    _address1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Address2
        {
            get => _address2;
            set
            {
                if (value != _address2)
                {
                    _address2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Tel1
        {
            get => _tel1;
            set
            {
                if (value != _tel1)
                {
                    _tel1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Tel2
        {
            get => _tel2;
            set
            {
                if (value != _tel2)
                {
                    _tel2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Phone1
        {
            get => _phone1;
            set
            {
                if (value != _phone1)
                {
                    _phone1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Phone2
        {
            get => _phone2;
            set
            {
                if (value != _phone2)
                {
                    _phone2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email1
        {
            get => _email1;
            set
            {
                if (value != _email1)
                {
                    _email1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email2
        {
            get => _email2;
            set
            {
                if (value != _email2)
                {
                    _email2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Site
        {
            get => _site;
            set
            {
                if (value != _site)
                {
                    _site = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    RaisePropertyChanged();
                }
            }
        }

        Store store;

        public ObservableCollection<User> Users { get; set; }

        public ICommand SelectImage { get; set; }
        public ICommand Edit { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Stores Message = Application.Current.Windows.OfType<Stores>().FirstOrDefault();

        public StoresViewModel()
        {
            LoadCommands();
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll());
                store = db.GetCollection<Store>(DBCollections.Stores.ToString()).FindById(1);
                Name = store.Name;
                Motto = store.Motto;
                Image = store.Image;
                Address1 = store.Address1;
                Address2= store.Address2;
                Tel1= store.Tel1;
                Tel2=store.Tel2;
                Phone1=store.Phone1;
                Phone2=store.Phone2;
                Email1=store.Email1;
                Email2=store.Email2;
                Site = store.Site;
                Notes = store.Notes;
            }
        }

        public void LoadCommands()
        {
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            Edit = new DelegateCommand(DoEdit, CanEdit);
        }

        private bool CanEdit()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoEdit()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                store = db.GetCollection<Store>(DBCollections.Stores.ToString()).FindById(1);
                store.Name = Name;
                store.Motto = Motto;
                store.Image = Image;
                store.Address1 = Address1;
                store.Address2 = Address2;
                store.Tel1 = Tel1;
                store.Tel2 = Tel2;
                store.Phone1 = Phone1;
                store.Phone2 = Phone2;
                store.Email1 = Email1;
                store.Email2 = Email2;
                store.Site = Site;
                store.Notes = Notes;
                store.EditDate = DateTime.Now;
                store.Editor = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id);
                db.GetCollection<Store>(DBCollections.Stores.ToString()).Update(store);
                Message.ShowMessageAsync("تمت العملية", "تم حفظ بيانات المحل بنجاح");
            }
        }

        private bool CanSelectImage()
        {
            return true;
        }

        private void DoSelectImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            var codecs = ImageCodecInfo.GetImageEncoders();
            dlg.Filter = string.Format("All image files ({1})|{1}|All files|*",
            string.Join("|", codecs.Select(codec =>
            string.Format("({1})|{1}", codec.CodecName, codec.FilenameExtension)).ToArray()),
            string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray()));
            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                Image = File.ReadAllBytes(filename);
            }
        }
    }
}