﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using XFDoggy.Helps;

namespace XFDoggy.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public readonly IPageDialogService _dialogService;

        #region Account
        private string account;
        /// <summary>
        /// Account
        /// </summary>
        public string Account
        {
            get { return this.account; }
            set { this.SetProperty(ref this.account, value); }
        }
        #endregion


        #region Password
        private string password;
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set { this.SetProperty(ref this.password, value); }
        }
        #endregion

        public DelegateCommand 登入Command { get; private set; }

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;

            _dialogService = dialogService;

            登入Command = new DelegateCommand(登入);
            Account = "001";
            Password = "901";
        }

        private async void 登入()
        {
            var fooResult = await AppData.DataService.AuthUserAsync(new Models.AuthUser
            {
                Account = Account,
                Password = Password,
            });

            if (fooResult.Status == true)
            {
                AppData.Account = Account;
                var fooItems = (await AppData.DataService.GetTravelExpensesAsync(AppData.Account)).ToList();
                AppData.AllTravelExpense = fooItems;
                await _navigationService.NavigateAsync("xf:///MainMDPage/NaviPage/TravelExpensesListPage");
            }
            else
            {
                await _dialogService.DisplayAlertAsync("錯誤", "帳號或者密碼錯誤", "確定");
            }
        }
    }
}
