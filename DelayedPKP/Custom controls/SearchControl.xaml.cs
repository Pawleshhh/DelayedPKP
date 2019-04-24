using DelayedPKP.ViewModel;
using System;
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

namespace DelayedPKP
{
    /// <summary>
    /// Logika interakcji dla klasy SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl, IConnectedWithFrame
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        #region TextProperty

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register(
                  "Text",
                  typeof(string),
                  typeof(SearchControl),
                  new PropertyMetadata(TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchControl searchControl = d as SearchControl;
            if (searchControl == null)
                throw new ArgumentException("Dependency object is not a type of SearchControl", "d");

            ((SearchControlViewModel)searchControl.DataContext).Text = (string)e.NewValue;
        }

        #endregion

        #region IconPathProperty

        public string IconPath
        {
            get
            {
                return (string)GetValue(IconPathProperty);
            }
            set
            {
                SetValue(IconPathProperty, value);
            }
        }

        public static readonly DependencyProperty IconPathProperty
            = DependencyProperty.Register(
                  "IconPath",
                  typeof(string),
                  typeof(SearchControl),
                  new PropertyMetadata(ImageSourceChanged));

        private static void ImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchControl searchControl = d as SearchControl;
            if (searchControl == null)
                throw new ArgumentException("Dependency object is not a type of SearchControl", "d");

            ((SearchControlViewModel)searchControl.DataContext).IconPath = (string)e.NewValue;
        }

        #endregion

        #region FrameProperty

        public Frame Frame
        {
            get
            {
                return (Frame)GetValue(FrameProperty);
            }
            set
            {
                SetValue(FrameProperty, value);
            }
        }

        public static readonly DependencyProperty FrameProperty
            = DependencyProperty.Register(
                  "Frame",
                  typeof(Frame),
                  typeof(SearchControl),
                  new PropertyMetadata(FrameChanged));

        private static void FrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchControl searchControl = d as SearchControl;
            if (searchControl == null)
                throw new ArgumentException("Dependency object is not a type of SearchControl", "d");

            //((SearchControlViewModel)searchControl.DataContext).Text = (string)e.NewValue;
        }


        #endregion

    }
}
