   M S B A M L   `   `   `  ����O  KHeThongQuanLyBaiDoXe, Version=2.1.0.1, Culture=neutral, PublicKeyToken=null;"clr-namespace:HeThongQuanLyBaiDoXeHeThongQuanLyBaiDoXe  5   	   U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nulleHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.WpfMaterialDesignThemes.Wpf 5   	   -    'HeThongQuanLyBaiDoXe.RegistrationWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:HeThongQuanLyBaiDoXe     5   	   XwpfHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.Wpf     5   	   FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   -   5          � Title$  Login��5
   	   $	��400��6   $	��600��6$    � 
ResizeMode$ NoResize=�60    � WindowStyle$
 None=�6F    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterScreen=�6�    � 	Resources 5      ;   5HeThongQuanLyBaiDoXe.PathStringToBitmapImageConverter $   PathStringToBitmapImageConverter)   #HeThongQuanLyBaiDoXe.MoneyConverter  MoneyConverter  moneyTemplate �� DataType �� 	Converter��%�   &	        &	      &	 &     7    5   
   61     5   
   6   �� 5   
   " ��6-   .R�5      �� 
���   ���6   �� *Value+8   6Y   5      5      .��5      � -   5        .+�5   
   ��  �� RadiusY$ 8��6   	 �� RadiusX$	 8��6    
�������6,   �� 5   
   $��Assets/tnutimg.jpg��6   $��	310,0,0,0q�6,   $��UniformToFill=�6?   �� 5   
   $��	310,0,0,0q�6   ��5      �� 5      
 �� EndPoint$
 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5      �  � Color$ 	#CC935E12��6#    � Offset$ 1Q�65   � 5      $ 	#CCEA4646��6#   � 5      $ 	#CCB89128��6#   $ 0.566Q�65   5      5      �� 5   
   $��20,10,290,10q�6   .+�5       �� $��
Đăng ký��6   $��Segoe UI Black)�6&   $��24(�6B   $
��0,10q�6P   $	��Top=�6^   $
��Left=�6v   �� 5!       �� Orientation$ 
Horizontal=�6   .+�5"      �� $
��Left=�6   $	��Top=�67   $
��Wrap=�6O   ���#707070�� 6y   $3�-Nếu bạn đã có tài khoản, vui lòng ��6}   6�   �� 5#      -   5        $��btnLogin��5#      $��Đăng nhập��6)    � Cursor$
 Hand��6=   $��Center=�6K   $��Right=�6f   ��5$      �� 5%       �� 
TargetType" ��6*   .R�5&      �� $��	Underline{�6(   �� 5'      5(      5)      ��5+      �� 5,       �� 
TargetType" ��6    .�5-      ��  �� Property � �� �� Value �	#FF2468AC6;   �� 5.       � ��$ 12(�69    �� Triggers 5/      P� 50   "    P� Property �@J�IsMouseOver P� Value . . �51   &   ��  � ��
 �  ��6C   52   #   54      56      58      �� 59      $
��Left=�6   $	��Top=�63   $
��Wrap=�6K   ���#707070�� 6u   $%�để sử dụng hệ thống.��6y   6�   �� 5;      $��0,10,0,0q�6   $ 
Horizontal=�6+   .+�5<      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$ Account=�6*   $��Center=�69   ���#7070706T   �� 5=      -   5        $��txtName��5=      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Họ và tên��6)   $	��120��6T   $��5,0,0,0q�6`   �� 5>      -   5        $��imageViewer��5>      $��30��6+   $��30��66   $
��Fill=�6B   $	��5,0q�6Q   ��5?      �� 5@       �� Path$ SrcImage�6"   # [  62    �� TargetNullValue 5A      �� 5B   "    �� 	UriSource$  /Assets/DeafultProfile.png?�6.   5D      5F      �� 5G      -   5        $��uploadProfileImage��5G      $��Tải ảnh lên...��63   $��Center=�6L   $��Right=�6g   $��90��6�   $��30��6�   $��0q�6�   $��0,0,0,0q�6�   5H      �� 5I      $	��0,0q�6   $ 
Horizontal=�6&   .+�5J        $ CardBulleted=�6*   $��Center=�6>   ���#7070706Y   �� 5K      -   5        $��txtBienKiemSoat��5K      $ Biển kiểm soát��61   $	��120��6a   $	��5,0q�6m   �� 5L      -   5        $��txtCode��5L      $ Mã thẻ gửi��6)   $	��120��6U   $	��5,0q�6a   5M      �� 5N      $	��0,0q�6   $ 
Horizontal=�6&   .+�5O        $ Office=�6*   $��Center=�68   ���#7070706S   �� 5P      -	   5        $��txtDep��5P      $ Lớp / Khoa��6(   $	��120��6T   $	��5,0q�6`   �� 5Q      -
   5        $��cbbDaNop��5Q      $  Số tiền nộp trước��6+   $	��120��6^   $	��5,0q�6j   
�������6w   5S      �� 5T      $ 
Horizontal=�6   .+�5U        $	 Key=�6*   $��Center=�65   ���#7070706P   G� 5V      -   5        $��
pwPassword��5V      $ Mật khẩu��60   $	��250��6Z   $	��5,0q�6f   5W      �� 5X      $ 
Horizontal=�6   .+�5Y        $ 	KeyChange=�6*   $��Center=�6;   ���#7070706V   G� 5Z      -   5        $��pwAcceptPassword��5Z      $ Xác nhận mật khẩu��66   $	��250��6i   $	��5,0q�6u   5[      �� 5\      $	��0,0q�6   $ 
Horizontal=�6&   .+�5]        $ Information=�6*   $��Center=�6=   ���#7070706X   �� 5^      -   5        $��	txtReason��5^      $ Lý do cấp tài khoản��6+   $	��250��6`   $	��5,0q�6l   5_      � 5`      .+�5a      �� $
��0,10q�6   $ 
Horizontal=�6+   .+�5b        $ Settings=�6.   $��Center=�6>   ���#7070706Y   �� 5c      -   5        $��cbbPhanQuyen��5c      $ Phân quyền��63   $	��250��6_   $	��5,0q�6k   
�������6x   5d      5e      �� 5f      -   5         ��Name$ tblErrorMessage��5f   $   $��20��6    J� OpacityMask
 �<�����	#FFE5572C6T   �� 5g      -   5        $��btnRegister��5g      $��Center=�6(   $��
ĐĂNG KÝ��6E   $��Impact)�6W   $��18(�6k   $	��120��6y   ���	#FF307CD6���	#FF307CD6$��0q�6�   6�   5h      �� 5i   
   -   5        $��btnThoat��5i      $��Right=�6!   $	��Top=�6=   $��5q�6U     MaterialDesignFlatButton#��[ 6`   .��5j        $ Close=�6&   5k      �� 5l   
   $��310,50,0,50q�6   .+�5m      �� $��FEE TNUT��6   
�������$��Center=�6;   $��48(�6X   $��Champagne & Limousines)�6f   $
��Bold"�6�   �� 5n      $8��2Hệ thống Quản lý bãi đỗ xe tự động��6   
�������$��Center=�6U   $	��280��6r   $��24(�6~   $��Champagne & Limousines)�6�   $
��Wrap=�6�    �� TextAlignment$ Center=�6�   $��0,50,0,0q�6�   �� 5o      $*��$GVHD: ThS. Nguyễn Thị Hải Ninh��6   
�������$��18(�6Q   $��Champagne & Limousines)�6_   $
��Wrap=�6�   $
 Left=�6�   $��
10,30,10,5q�6�   �� 5p      $#��SVTH : Nguyễn Thị Huyền��6   
�������$��18(�6J   $��Champagne & Limousines)�6X   $
��Wrap=�6�   $
 Left=�6�   $
��10,5q�6�   �� 5q      $��Nguyễn Thị Nhã��6   
�������$��18(�6A   $��Champagne & Limousines)�6O   $
��Wrap=�6w   $
 Left=�6�   $��	67,0,10,0q�6�   5r      5s      5t      