   M S B A M L   `   `   `  ����H  DSignature.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Signature.WPFSignature.WPF  5   	       Signature.WPF.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Signature.WPF     5   	     � Title$  eID Signature - POC��5   	   $	��700��6%   $	��800��62    � 
ResizeMode$ NoResize=�6>   .��5	      �  � ColumnDefinitions 5   
   �� 5      $
��0.5* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $
��0.5* �6    � RowDefinitions 5   
   �� 5      $	��63* �6   �� 5      $
��137* �6   �� 5      $	��51* �6   �� 5      $
��418* �6   .+�5   
   �� $��./images/PXLLogo.png��6   �� 5   
   $��2��6   $��Center=�6$   $��Center=�6A   $��24(�5      $
��Bold"�6   ���#6ca336$��1��6E   $��74,10,74,11q�6U   $��42��6j   $	��380��6v   .��6�   " eID Signature - Proof of Concept5      �� 5"   
   $��1��6    �� Orientation$ 
Horizontal=�6%   $��Center=�6>   $��1��6Y   $��	0,29,0,30q�6f   $��78��6y   .+�5#      �� $ Vertical=�6   .+�5$      �� $
��Bold"�6   .��6*   Naam:61   �� 5%      $
��Bold"�6   .��6*   
Voornamen:66   �� 5&      $
��Bold"�6   .��6*   	Geslacht:65   5'      �� 5(      $ Vertical=�6   .+�5)      �� -   5         ��Name$ 
lblSurname��5)      .��6,   /6/   �� 5*      -   5        $ lblFirstnames��5*      .��6/   /62   �� 5+      -   5        $ 	lblGender��5+      .��6+   /6.   5,      5-      �� 5.   
   $��2��6   $ 
Horizontal=�6%   $��Center=�6>   $��1��6Y   $��	0,29,0,30q�6f   $��78��6y   .+�5/      �� $ Vertical=�6   .+�50      �� $
��Bold"�6   .��6*   Geboorteplaats:6;   �� 51      $
��Bold"�6   .��6*   Geboortedatum:6:   �� 52      $
��Bold"�6   .��6*   Nationaliteit:6:   53      �� 54      $ Vertical=�6   $��1��60   .+�55      �� -   5        $ lblLocationOfBirth��55      .��64   /67   �� 56      -   5        $ lblDateOfBirth��56      .��60   /63   �� 57      -   5        $ lblNationality��57      .��60   /63   58      59      �� 5<   
   -   5         ��Name$ btnSign��5<      $��DIGITAAL TEKENEN��6"   $��2��5=      $��1��6   $��10q�6.   $
��Bold"�5>      
���   �
�������6I   �� 5A   
   -   5        $ 
lblLoading��5A      $#��Bezig met digitaal tekenen...��6$   $��2��6L   $��2��6Y   $��Center=�5B      
���  ��$y�Hidden=�6<   �� 5E   
   -	   5        $ lblConfirmation��5E      $��2��6)   $��2��66   $��Center=�5F      ���#6ca336$y�Hidden=�6@   (  "System.Windows.Controls.WebBrowser  5I   
   -
   5          Name$ pdfWebViewer��5I      $��3��6+   $��1��68   $��2��6H   $��0,0,0,20q�5J      6)   5L      5M      