   M S B A M L   `   `   `  ����H  DwpfSlidePanel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:wpfSlidePanelwpfSlidePanel  5   	   F BwpfControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP7clr-namespace:wpfControls.Controls;assembly=wpfControlswpfControls.Controls 5   	       wpfSlidePanel.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:wpfSlidePanel     5   	   Lcontrols7clr-namespace:wpfControls.Controls;assembly=wpfControls     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5      � -   5         �Name$ parent��5      .+�5)   
   %  wpfControls.Controls.SlidePanel  -   5          Name$ sliderPanel��5)         SliderWidth$	 300Q�5*         AnimationSpeed$	 300Q�5+         SliderLocation$
 Left=�5,      .��5-      � ���#242527 � RowDefinitions 5.      �� 5/      $
��Auto �6$   �� 50      $��* �6$   .+�52      � $��20q�6    53      �� 54      $
��auto �6(   �� 55      $
��auto �6(   .+�57      �� $��./Images/file.png��6   $
��auto��58      $	��100��59      �� 5:      $��1��6    $��ParkSangHoon��5;       $��15(�5<       $
��Bold"�5=       ���#F0F0F0$��Center=�5?       5@      �� 5A      $��1��6    �� Orientation$ Vertical=�5B      ���#FFFFFF � 	Resources 5D      	 �� 
TargetType
 �� Property �� Value��%�   '��       �� 5E      "	 ��6    .�5F      �� 
 � ��$ 0q�6@   �� 5G      
 � ��
 ���� 6;   �� 5H      
 � ��$ 15q�68   �� 5I      
 � ��$ 17(�69   �� 5J      
 � ��$
 Left=�6K   5K      5L      .+�5M      �� $��Setting��6   �� 5N      $��고객센터��6   �� 5O      $��	이벤트��6   �� 5P      $��회원가입��6   
����  �$
��Bold"�5R      ��5S      �� *SignupCommand+5T      5V      5W         wpfControls.Controls.Modal  5X   
      ShadowColor$
 Blue��6      IsOpen 5Y      �� *IsSignupModalOpen+.��5Z      � $	��500��6   $	��500��6   5\      5]      � 5^   
   ���#F8F8F8 5_      �� 5`      $
��Auto �6    �� 5a      $��* �6    �� 5b      $
��Auto �6    .+�5d      �� $��0��6   ���#003F5C$��50��5f      ._�5g      �� $ 
Horizontal=�6   $��Center=�5h      $��10q�5i      .+�5j      �� -   5        $	��○��5j      $��20(�5k      $
��Bold"�5l      ���#FFFFFF$	��5 0q�5n      $��3 0 0 3q�5o      
������ 5q      �� 5r      $��My App��6    $��20(�6.   $
��Bold"�5s       ���#FFFFFF62   5t      5u      � 5v      $��1��6   $��20q�6     5w      �� 5x      $
��Auto �6$   �� 5y      $
��Auto �6$   .+�5{      �� $��0��6   $��My Page��6)   $��24(�5|      $
��Bold"�5}      �� 5~      $��1��6   $ 
Horizontal=�5      $��Center=�5�      $��20q�5�      .+�5�      �� 
�������$��1q�5�       �� CornerRadius$ 5��5�      $��10q�5�      ._�5�      �� $ Vertical=�6%   $��5q�5�   %   .+�5�      �� $��./Images/opened-drive.png��6$   $	��150��5�   !   $	��150��6-   �� 5�      $��My Image��6(    �� TextAlignment$ Center=�5�       5�      5�      �� 5�      
�������$��1q�5�      $ 5��5�      $��10q�5�      ._�5�      �� $ Vertical=�6%   $��5q�5�   %   .+�5�      �� $��./Images/file.png��6$   $	��150��5�   !   $	��150��6-   �� 5�      $��My Image��6(   $ Center=�5�       5�      5�      �� 5�      
�������$��1q�5�      $ 5��5�      $��10q�5�      ._�5�      �� $ Vertical=�6%   $��5q�5�   %   .+�5�      �� $��./Images/closed-drive.png��6$   $	��150��5�   !   $	��150��6-   �� 5�      $��My Image��6(   $ Center=�5�       5�      5�      �� 5�      
�������$��1q�5�      $ 5��5�      $��10q�5�      ._�5�      �� $ Vertical=�6%   $��5q�5�   %   .+�5�      �� $ ��./Images/opened-folder.png��6$   $	��150��5�   !   $	��150��6-   �� 5�      $��My Image��6(   $ Center=�5�       5�      5�      5�      5�      �� 5�      $��2��6   ���#003F5C$��30��5�      ._�5�      �� $��@2023 My Company��6   $��12(�64   $��Center=�5�      ���#FFFFFF5�      5�      5�      5�      5�      