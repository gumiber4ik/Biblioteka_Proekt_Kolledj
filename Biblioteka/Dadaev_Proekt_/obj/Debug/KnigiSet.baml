   M S B A M L   `   `   `  ����I  EDadaev_Proekt_, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Dadaev_Proekt_Dadaev_Proekt_  5   	       Dadaev_Proekt_.KnigiSet   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Dadaev_Proekt_     5   	     � Title$  Возврат книги��5   	   $	��450��6   $	��400��6,    � Icon$ ICO/app.png��68   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��10q�6V   .+�5      �� $��/ICO/logo.png��6   $��50��6+   $��50��66   $��
0,10,10,10q�6B   �� 5      $��Библиотека��6   $��24(�6*   $
��Bold"�68   $��Center=�6J   5      �� 5   
   $��16(�6   $&�� Выбрать студента:��6"   $��85,219,-85,59q�6;   $��1��6R   �� 5   
   -   5         ��Name$ 
cbStudents��5      $��85,244,85,13q�6(    �� Text$' !Выберите студента��6>    J� RenderTransformOrigin$ 0.553,0.173&�6�   $��1��6�   z�5      V� 5      .~�5      ��  �� ScaleY$ 1Q�6%   �� 5      �� 5       T� 5!      5"      5$      �� 5'   
   -   5         ��Name$ lvBooks��5'      $��1��6$   $��10,10,10,85q�61   	 �� View	 5(      �� 5)      .P�5*      �� $!��Название книги��6%   
 �� DisplayMemberBinding
 6=   �� *Title+�� 5+      $��Дата возврата��6%   
 6<   �� *
ReturnDate+ �� StringFormat$ {0:dd/MM/yyyy}���� 5,      $��
Штраф��6%   
 64   �� *Fine+5-      5/      �� 52   
   -   5        $��2��52      $%��Оформить возврат��6   $��10q�69   6E   53      54      