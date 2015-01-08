﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NETPath.Projects.WebApi;

namespace NETPath.Interface.WebApi.Service
{
	internal partial class HttpClientConfig : Grid
	{
		public WebApiHttpConfiguration HttpConfig { get { return (WebApiHttpConfiguration)GetValue(HttpConfigProperty); } set { SetValue(HttpConfigProperty, value); } }
		public static readonly DependencyProperty HttpConfigProperty = DependencyProperty.Register("HttpConfig", typeof(WebApiHttpConfiguration), typeof(HttpClientConfig));

		public HttpClientConfig(WebApiHttpConfiguration HttpConfig)
		{
			InitializeComponent();

			this.HttpConfig = HttpConfig;
			DataContext = this.HttpConfig;
		}
	}
}