﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Custom.Wpf.Global.Controls;
public enum IconMode
{
    None,
    Icon,
    Image,
}
public enum ImageType
{
    None,
    AppStore,
    Arsenal,
    Benz,
    Bmw,
    Chelsea,
    CrystalPalace,
    Disney,
    DisneyPlus,
    Everton,
    Honda,
    Hyundai,
    LeicesterCity,
    ManchesterCity,
    ManchesterUnited,
    NewCastle,
    Porsche,
    Prime,
    QQ,
    SouthHampton,
    Spotify,
    Sunderland,
    SwanseaCity,
    Tesla,
    Tinder,
    Tottenham,
    WestBromwitchAlbion,
}
public enum IconType
{
    None,
    Twitter,
    CheckDecagram,
    Email,
    EmailOutline,
    BellOutline,
    DotsHorizontal,
    Instagram,
    Facebook,
    Linkedin,
    Youtube,
    Link,
    LinkBox,
    LinkVariant,
    Domain,
    MapMaker,
    MapMarkerOutline,
    Microsoft,
    Apple,
    Google,
    Netflix,
    Star,
    AccountMultipleOutline,
    Image,
    Heart,
    HeartOutline,
    Pin,
    CardMultiple,
    CardMultipleOutline,
    Comment,
    CommentOutline,
    Close,
    CheckCircle,
    Check,
    Crop,
    MicrosoftVisualStudio,
    MoveOpenPlay,
    Menu,
    GoogleTranslate,
    EyedropperVariant,
    CogRefreshOutline,
    MonitorShimmer,
    ChevronRight,
    ChevronDown,
    CursorPointer,
    CalendarMonth,
    WindowMinimize,
    Web,
    Palette,
    Contentpaste,
    Checkbold,
    FolderOpenOutline,
    FolderOpen,
    FolderRable,
    Maximize,
    Resize,
    SelectAll,
    ArrowLeftBold,
    ArrowRightBold,
    ArrowUpBold,
    ConsoleLine,
    Plus,
    ArrowAll,
    MicrosoftWindows,
    ArrowDownBox,
    TextBox,
    Folder,
    FolderOutline,
    DesktopClassic,
    Harddisk,
    File,
    FileWord,
    FileCheck,
    FileZip,
    FilePdf,
    FileImage,
    DotsHorizontalCircle,
    Home,
    HomeOutline,
    PlusBox,
    Database,
    DatabasePlus,
    Delete,
    Grid,
    AccountGroup,
    PokerChip,
    Creditcardchip,
    CreditcardchipOutline,
    Memory,
    Account,
    HomeCircle,
    HomeCircleOutline,
    Cash,
    Cash100,
    CashMulti
}

public class CustomIcon : Label
{
    private Viewbox _viewbox;
    private Image _image;

    public static DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(IconMode), typeof(CustomIcon), new PropertyMetadata(IconMode.None));
    public static DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(IconType), typeof(CustomIcon), new PropertyMetadata(IconType.None, IconPropertyChanged));
    public static DependencyProperty ImageProperty = DependencyProperty.Register("Iamge", typeof(ImageType), typeof(CustomIcon), new PropertyMetadata(ImageType.None, ImagePropertyChanged));

    public static DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(SolidColorBrush), typeof(CustomIcon), new PropertyMetadata(Brushes.Silver));
    public static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(CustomIcon), new PropertyMetadata(null));
    public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(CustomIcon), new PropertyMetadata(null));

    public IconMode Mode
    {
        get => (IconMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public IconType Icon
    {
        get => (IconType)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ImageType Image
    {
        get => (ImageType)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public SolidColorBrush Fill
    {
        get => (SolidColorBrush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public Geometry Data
    {
        get => (Geometry)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    private static void IconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        CustomIcon customIcon = (CustomIcon)d;
        string geometryData = Design.Geometries.GeometryConverter.GetData(customIcon.Icon.ToString());

        customIcon.Data = Geometry.Parse(geometryData);
        customIcon.Mode = IconMode.Icon;
    }

    private static void ImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        CustomIcon jamesIcon = (CustomIcon)d;
        string base64 = Design.Images.ImageConverter.GetData(jamesIcon.Image.ToString());

        byte[] binaryData = Convert.FromBase64String(base64);

        BitmapImage bi = new BitmapImage();
        bi.BeginInit();
        bi.StreamSource = new MemoryStream(binaryData);
        bi.EndInit();

        jamesIcon.Source = bi;
        jamesIcon.Mode = IconMode.Image;
    }
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }
    static CustomIcon()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomIcon), new FrameworkPropertyMetadata(typeof(CustomIcon)));
    }
}
