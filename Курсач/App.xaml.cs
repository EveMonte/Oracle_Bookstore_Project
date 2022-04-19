﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using Курсач.Models;

namespace Курсач
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
		//public static LIBRARYEntities db = new LIBRARYEntities();
		public static User currentUser;
		public static Notifier notifier;
		private static List<CultureInfo> m_Languages = new List<CultureInfo>();

		public static List<CultureInfo> Languages
		{
			get
			{
				return m_Languages;
			}
		}
		public static void CreateNotifier(Window window)
        {
			notifier = new Notifier(cfg =>
			{
				cfg.PositionProvider = new WindowPositionProvider(
					parentWindow: window,
					corner: Corner.BottomRight,
					offsetX: 10,
					offsetY: 10);

				cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
					notificationLifetime: TimeSpan.FromSeconds(5),
					maximumNotificationCount: MaximumNotificationCount.FromCount(5));

				cfg.Dispatcher = Application.Current.Dispatcher;
			});
		}

		private void OnStartup(object sender, StartupEventArgs e)
		{
			MainWindow view = new Курсач.MainWindow(); // создали View
			CreateNotifier(view);
			view.Show();
		}

		private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
		{
			Language = Курсач.Properties.Settings.Default.DefaultLanguage;
		}

		private void App_LanguageChanged(Object sender, EventArgs e)
		{
			Курсач.Properties.Settings.Default.DefaultLanguage = Language;
			Курсач.Properties.Settings.Default.Save();
		}
		public App()
		{
			m_Languages.Clear();
			m_Languages.Add(new CultureInfo("ru-RU"));
			m_Languages.Add(new CultureInfo("en-En"));
			App.LanguageChanged += App_LanguageChanged;
		}
		//Евент для оповещения всех окон приложения
		public static event EventHandler LanguageChanged;

		public static CultureInfo Language
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

				//1. Меняем язык приложения:
				System.Threading.Thread.CurrentThread.CurrentUICulture = value;

				//2. Создаём ResourceDictionary для новой культуры
				ResourceDictionary dict = new ResourceDictionary();
				switch (value.Name)
				{
					case "en-EN":
						dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
						break;
					default:
						dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
						break;
				}

				//3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
				ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
											  where d.Source != null && d.Source.OriginalString.StartsWith("Resources")
											  select d).First();
				if (oldDict != null)
				{
					int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
					Application.Current.Resources.MergedDictionaries.Remove(oldDict);
					Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
				}
				else
				{
					Application.Current.Resources.MergedDictionaries.Add(dict);
				}

				//4. Вызываем евент для оповещения всех окон.
				LanguageChanged(Application.Current, new EventArgs());
			}
		}
	}
}
