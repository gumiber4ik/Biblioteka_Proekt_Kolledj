   M S B A M L   `   `   `  ����I  EDadaev_Proekt_, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Dadaev_Proekt_Dadaev_Proekt_  5   	   !    Dadaev_Proekt_.userbooklist   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Dadaev_Proekt_     5   	     � Title$0  *Книги взятые студентом��5   	   $	��450��6(   $	��800��65    � Icon$ ICO/app.png��6A   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� $��0��6    �� Orientation$ 
Horizontal=�6)   $��Center=�6B   $��10q�6]   .+�5      �� $��/ICO/logo.png��6   $��50��6/   $��50��6:   $��0,0,10,0q�6F   �� 5      $��Библиотека��6   $��24(�6.   $
��Bold"�6<   $��Center=�6N   5      �� 5      $��1��6   $ 
Horizontal=�6)   $
��Left=�6B   $��Center=�6]   $��10q�6x   �� 5!      -   5        $��Все книги��5!      $��1��6)   $��
451,34,0,1q�69   6M   �� 5"      -   5        $+��%Книги задолженность��5"      $��1��63   $��316,34,132,1q�6C   6Y   5#      � 5%   
   $��1��6   .+�5&      �� -   5        $��ListViewBooks��5&      $��1��6-   ���	#FF2E4861$��Center=�6Q   $��Disabled=�6n   $	��800��6�   $��0,40,0,0q�6�   ��5'      �� 5(      .R�5*      �  � Orientation$ 
Horizontal=�6$   $��Center=�6=   5+      ��5.      �� 5/      .R�51      �� 
�������$��1q�64    �� CornerRadius$ 8��6H   
������� �� Padding$ 10q�6l   $��10q�6y   $	��250��6�   ._�52      �� .+�53   "   �� $��16(�6C   $
��Bold"�6Q   $��0,0,0,5q�6c   ���	#FF2E4861��6,   �� *Title+6t   �� 54   "   $*��$Автор произведения:��6,   $��16(�6G   $
��Bold"�6U   $��0,0,0,5q�6g   ���	#FF2E48616x   �� 55   "   $��14(�6D   $��0,0,0,5q�6R   ���	#FF2E4861��6,   �� *Author+6c   �� 56   "   $0��*Индификационный номер:��6,   $��16(�6J   $
��Bold"�6X   $��0,0,0,5q�6j   ���	#FF2E48616{   �� 57   "   $��14(�6B   $��0,0,0,5q�6P   ���	#FF2E4861��6,   �� *ISBN+6a   �� 58   "   $ ��Дата возврата:��6,   $��16(�6B   $
��Bold"�6P   $��0,0,0,5q�6b   ���	#FF2E48616s   �� 59   "   $��14(�6j   $��0,0,0,5q�6x   ���	#FF2E4861��6,   �� *Return_Date+ �� StringFormat$ {0:dd/MM/yyyy}��6�   5:      5;      5<      5?      5@      5A      5B      