   M S B A M L   `   `   `  ����O  KHeThongQuanLyBaiDoXe, Version=2.1.0.1, Culture=neutral, PublicKeyToken=null;"clr-namespace:HeThongQuanLyBaiDoXeHeThongQuanLyBaiDoXe  5   	   (    "HeThongQuanLyBaiDoXe.MessageWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:HeThongQuanLyBaiDoXe     5   	   U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Message Window��5
   	    � 
ResizeMode$ NoResize=�6    $	��300��66   $	��150��6B    � WindowStyle$
 None=�6O    � AllowsTransparency . 
������  � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?�6%   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5      $u opack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Green.xaml?�6%   �� 5      $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?�6%   %    5      .��5      � $��Center=�6   .+�5   
   �� 
������� �� RadiusX$ 8��6!   	 �� RadiusY$	 8��6-   5      �� 5   
   
 �� Orientation$
 
Horizontal=�6   $��15q�6.   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  -   5        $��packIcon��5   &   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$ Settings=�66      PrimaryHueMidBrush#��[  6F   $��50��6w   $��50��6�   �� 5      -   5        $��tblThongBao��5      
�������$	��200��6=   $
��15 5q�6I   $
��Wrap=�6W   Cho phép gửi xe?5      5      �� 5   
   $
 
Horizontal=�6   $��Right=�6.   $��Bottom=�6J   $��15q�6e   .+�5       �� -   5        $��	btnTuChoi��5         MaterialDesignFlatButton#���  6&   $��4q�6Y   $��Center=�6d   .��6�   Từ chối5"      �� 5#      -   5        $��btnChapNhan��5#      #���  6(   $��4q�6[   $��Center=�6f   .��6�   Chấp nhận5%      5&      5'      5)      