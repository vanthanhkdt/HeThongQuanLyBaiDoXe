   M S B A M L   `   `   `  ����O  KHeThongQuanLyBaiDoXe, Version=2.1.0.1, Culture=neutral, PublicKeyToken=null;"clr-namespace:HeThongQuanLyBaiDoXeHeThongQuanLyBaiDoXe  5   	   U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nulleHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.WpfMaterialDesignThemes.Wpf 5   	   &     HeThongQuanLyBaiDoXe.LoginWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:HeThongQuanLyBaiDoXe     5   	   XwpfHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.Wpf     5   	   FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	     � Title$  Login��5
   	   $	��400��6   $	��600��6$    � 
ResizeMode$ NoResize=�60    � WindowStyle$
 None=�6F    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterScreen=�6�   .��5      � -   5        .+�5   
   ��  �� RadiusY$ 8��6    �� RadiusX$ 8��6    
�������6,   �� 5   
   $��Assets/tnutimg.jpg��6   $��	310,0,0,0q�6,   $��UniformToFill=�6?   �� 5   
   $��	310,0,0,0q�6   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5      � 	 � Color$	 	#CC935E12��6#   
 � Offset$
 1Q�65   � 5      $	 	#CCEA4646��6#   � 5      $	 	#CCB89128��6#   $
 0.566Q�65   5      5      �� 5   
   $��20,10,290,10q�6   .+�5      �� $��Đăng nhập��6   $��Segoe UI Black)�6(   $��24(�6D   $
��0,10q�6R   $	��Top=�6`   $
��Left=�6x   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� $
��Left=�6   $	��Top=�67   $
��Wrap=�6O   ���#707070�� 6y   $1�+Bạn không có tài khoản? bạn cần ��6}   6�   �� 5      -   5        $��	btnSignIn��5      $��Tạo tài khoản��6*    � Cursor$
 Hand��6B   $��Center=�6P   $��Right=�6k   ��5      �� 5       �� 
TargetType" ��6*   .R�5      �� $��	Underline{�6(   �� 5       5!      5"      ��5$      �� 5%       �� 
TargetType" ��6    .�5&      ��  �� Property � �� �� Value �	#FF2468AC6;   �� 5'       � ��$ 12(�69    �� Triggers 5(      P� 5)   "    P� Property �@J�IsMouseOver P� Value . . �5*   &   ��  � ��
 �  ��6C   5+   #   5-      5/      51      �� 52      $
��Left=�6   $	��Top=�63   $
��Wrap=�6K   ���#707070�� 6u   $(�" để đăng nhập hệ thống.��6y   6�   �� 53      $
��0,15q�6   $ 
Horizontal=�6'   .+�54      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$ Account=�6*   $��Center=�69   ���#7070706T   �� 55      -   5        $��txtID��55      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$  Biển kiểm soát / CMND��6'    %  !MaterialDesignFloatingHintTextBox#��[  6^   $	��250��6�   $��5q�6�   56      �� 57      $ 
Horizontal=�6   .+�58        $	 Key=�6*   $��Center=�65   ���#7070706P   G� 59      -   5        $��
pwPassword��59      $ Mật khẩu��60    ) %MaterialDesignFloatingHintPasswordBox#��[ 6Z   $	��250��6�   $��5q�6�   6�   5:      � 5;      .+�5<      �� -   5        $��chkRememberPW��5<      $��Nhớ tên đăng nhập��60   $
��5,10q�6L   ���#7070706�   �� 5=      $��Quên mật khẩu?��6   $
 Hand��62   $��Center=�6@   $��Right=�6[   $��10q�6w   ��5>      �� 5?      " ��6*   .R�5@      �� $��	Underline{�6(   �� 5A      5B      5C      ��5E      �� 5F      " ��6    .�5G      ��  � �� �	#FF2468AC6;   �� 5H       � ��$ 12(�69    5I      P� 5J   "    �@J�IsMouseOver . . �5K   &   ��  � ��
 �  ��6C   5L   #   5N      5P      5R      �� 5S      -   5         ��Name$ tblErrorMessage��5S   $   $��20��6    J� OpacityMask
 �<�����	#FFE5572C6T   �� 5T      -   5        $��btnLogin��5T      $��Center=�6%   $��ĐĂNG NHẬP��6B   $��Impact)�6V   $��18(�6j   $	��120��6x   ���	#FF307CD6���	#FF307CD6$	��0,5q�6�   6�   �� 5U      $��10,15q�6   $��./Assets/logo.png��6$   $	��260��6?   $��Center=�6K   $��Center=�6f   5V      �� 5W   
   -   5        $��btnThoat��5W      $��Right=�6!   $	��Top=�6=   $��5q�6U     MaterialDesignFlatButton#��[ 6`   .��5X        $ Close=�6&   5Y      �� 5Z   
   $��310,50,0,50q�6   .+�5[      �� $��FEE TNUT��6   
�������$��Center=�6;   $��48(�6X   $��Champagne & Limousines)�6f   $
��Bold"�6�   �� 5\      $8��2Hệ thống Quản lý bãi đỗ xe tự động��6   
�������$��Center=�6U   $	��280��6r   $��24(�6~   $��Champagne & Limousines)�6�   $
��Wrap=�6�    �� TextAlignment$ Center=�6�   $��0,50,0,0q�6�   �� 5]      $*��$GVHD: ThS. Nguyễn Thị Hải Ninh��6   
�������$��18(�6Q   $��Champagne & Limousines)�6_   $
��Wrap=�6�   $
 Left=�6�   $��
10,30,10,5q�6�   �� 5^      $#��SVTH : Nguyễn Thị Huyền��6   
�������$��18(�6J   $��Champagne & Limousines)�6X   $
��Wrap=�6�   $
 Left=�6�   $
��10,5q�6�   �� 5_      $��Nguyễn Thị Nhã��6   
�������$��18(�6A   $��Champagne & Limousines)�6O   $
��Wrap=�6w   $
 Left=�6�   $��	67,0,10,0q�6�   5`      5a      5b      