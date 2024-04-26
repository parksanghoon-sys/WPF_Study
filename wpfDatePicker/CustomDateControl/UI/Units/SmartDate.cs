using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CustomDateControl.UI.Units
{
    public class SmartDate : Control
    {
        private Popup _popup;
        private CalendarSwitch _switch;
        private CalendarBox _listbox;
        public bool KeepPopupOpen
        {
            get { return (bool)GetValue(KeepPopupOpenProperty); }
            set { SetValue(KeepPopupOpenProperty, value); }
        }
        
        public static readonly DependencyProperty KeepPopupOpenProperty =
            DependencyProperty.Register("KeepPopupOpen", typeof(bool), typeof(SmartDate), new PropertyMetadata(true));
        public DateTime CurrentMonth
        {
            get { return (DateTime)GetValue(CurrentMonthProperty); }
            set { SetValue(CurrentMonthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMonthProperty =
            DependencyProperty.Register("CurrentMonth", typeof(DateTime), typeof(SmartDate), new PropertyMetadata(null));

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeletedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(SmartDate), new PropertyMetadata(null));
        static SmartDate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SmartDate), new FrameworkPropertyMetadata(typeof(SmartDate)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = (Popup)GetTemplateChild("PART_Popup");
            _switch = (CalendarSwitch)GetTemplateChild("PART_Switch");
            _listbox = (CalendarBox)GetTemplateChild("PART_ListBox");
            ChevronButton leftButton = (ChevronButton)GetTemplateChild("PART_Left");
            ChevronButton rightButton = (ChevronButton)GetTemplateChild("PART_Right");

            _popup.Closed += _popup_Closed;
            _switch.Click += _switch_Click;
            _listbox.MouseLeftButtonUp += _listbox_MouseLeftButtonUp;

            leftButton.Click += (sender, e) => MoveMonthClick(-1);
            rightButton.Click += (sender, e) => MoveMonthClick(1);
        }

        private void MoveMonthClick(int month)
        {
            GenerateCalendar(CurrentMonth.AddMonths(month));
        }

        private void _listbox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(_listbox.SelectedItem is CalendarBoxItem selected)
            {
                SelectedDate = selected.Date;
                GenerateCalendar(selected.Date);
                _popup.IsOpen = KeepPopupOpen;
            }
        }

        private void _switch_Click(object sender, RoutedEventArgs e)
        {
            if(_switch.IsChecked == true)
            {
                _popup.IsOpen = true;
                GenerateCalendar(SelectedDate ?? DateTime.Now);
            }
        }

        private void _popup_Closed(object? sender, EventArgs e)
        {
            _switch.IsChecked = IsMouseOver;
        }
        private  void GenerateCalendar(DateTime current)
        {
            if (current.ToString("yyyyMM") == CurrentMonth.ToString("yyyyMM")) return;

            CurrentMonth = current;
            _listbox.Items.Clear();
            DateTime fDayOfMonth = new(current.Year, current.Month, 1);
            DateTime lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

            int fOffset = (int)fDayOfMonth.DayOfWeek;
            int lOffset  = 6- (int)lDayOfMonth.DayOfWeek;

            DateTime fDay = fDayOfMonth.AddDays(-fOffset);
            DateTime lDay = lDayOfMonth.AddDays(lOffset);

            for (DateTime day = fDay; day <= lDay; day = day.AddDays(1))
            {
                CalendarBoxItem boxItem = new();
                boxItem.Date = day;
                boxItem.DateFormat = day.ToString("yyyyMMdd");
                boxItem.Content = day.Day;
                boxItem.IsCurrentMonth = day.Month == current.Month;

                _listbox.Items.Add(boxItem);
            }
            if (SelectedDate != null)
            {
                _listbox.SelectedValue = SelectedDate.Value.ToString("yyyyMMdd");
            }
        }
    }
}
