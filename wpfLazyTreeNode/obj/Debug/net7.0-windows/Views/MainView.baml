   M S B A M L   `   `   `  ����J  FwpfLazyTreeNode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:wpfLazyTreeNode.ViewswpfLazyTreeNode.Views  5   	   ?$clr-namespace:wpfLazyTreeNode.ModelswpfLazyTreeNode.Models  5   	   $    wpfLazyTreeNode.Views.MainView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:wpfLazyTreeNode.Views     5   	   7models$clr-namespace:wpfLazyTreeNode.Models     5   	     � Title$  MainView��5	   	   $	��450��6   $	��800��6'   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   � .+�5      R� -   5         R�Name$ trvRoot��5      ��6(   �� *	PathNodes+ � 	Resources 5       �� 
TargetType �� Property �� Value �� Mode��%}   'Q�       �� 5      " Q�6   .�5      ��  �@Q�
IsExpanded 5      �� *
IsExpanded+$ TwoWay=�5      5      ��5      �� 5       �� DataType)   #wpfLazyTreeNode.Models.LazyTreeNode"  6/   	 �� ItemsSource	 5   +   �� *
Children+.R�5      �� 
 �� Orientation$
 
Horizontal=�6%   .+�5      ��  ��Name$ 	treeImage��6$   $��24��67   $��24��5       ��5       �� *ClosedImage+�� 5      ��6(   �� *Text+5       �� Triggers 5       �� 5!       �� Value$
 True��6I    �� Binding 6*   �� *
IsExpanded+.a�5"   "   ��  �� 
TargetName$ 	treeImage��6)    5$   %   �� *OpenedImage+ � ��5%      5'      5)      5*      � 5+   
   $��1��6   .+�5,      �� -   5         ��Name$ btnTest��5,      $
��Test��6&   $��50��65   6@   5-      5.      5/      