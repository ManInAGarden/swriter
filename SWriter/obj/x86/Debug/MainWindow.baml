   M S B A M L   `   `   `  ����F  BSWriter, Version=1.0.3.36060, Culture=neutral, PublicKeyToken=null!clr-namespace:SWriterSWriter  5   	       SWriter.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   (customclr-namespace:SWriter     5   	   -   5          � Title$  SWriter��5   	    � WindowState$ 	Maximized=�6    � WindowStyle$
 None=�61   
���   � � Icon$( "/SWriter;component/Application.ico��5   ,    J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$ ApplicationCommands.New��5
      5        5   
   -   5        $ ApplicationCommands.Open��5      5        5   
   -   5        $ ApplicationCommands.Close��5      5        5   
   -   5        $ ApplicationCommands.Save��5      5        5   
   -   5        $  ApplicationCommands.SaveAs��5      5        5   
   -   5        $ ApplicationCommands.Print��5      5        5   
   -   5           SWriter.MainWindow   Search# Z 5      5        5!   
   -	   5           SetBold# Z 5!      5#        5$   
   -
   5           	SetItalic# Z 5$      5&        5(   
   -   5        	   Settings# Z	 5(      5)        5+   
   -   5        
   About# Z
 5+      5,        5-   
   -   5           Help# Z 5-      5.       J� InputBindings 52      �� 53   
   
 �� Key$ Q��6    �� 	Modifiers$ Controlf�54       �� Command$ ApplicationCommands.Close��55      �� 56   
   $ S��6   $ Controlf�57      $ ApplicationCommands.Save��58      �� 59   
   $ B��6   $ Controlf�5:      # Z 5;      �� 5<   
   $ I��6   $ Controlf�5=      # Z 5>      �� 5?   
   $ F��6   $ Controlf�5@      # Z 5A       � 	Resources 5D       �� 
TargetType �� BasedOn �� Property �� Value��%�   'o�       7o�'m� M     7m��� 5E   
   " o�6   8   .�5G      ��  � ��$ 14(�6)   5H      �� 5I   
   " m�6   8   .�5K      ��  � ��$ 14(�6)   5L      5M      .��5O      � -   5        $��mainGrid��5O   )   $��Stretch=�6   $��Stretch=�69   $��0,5,5,5q�6U    � ColumnDefinitions 5P   
   �� 5R      $	��100��6    � RowDefinitions 5U   
   �� 5V      $��10��6   $
��Auto �6+   �� 5W      $	��250��6   �� 5X      $
��Auto �6   .+�5[   
   o� -   5        $��mainMenu��5[   ,   $��0��6   $��0��6   .@�5\      m� -   5        $��Datei��5\      .@�5]      m� $	��Neu��6    m� Command$	 New��6(   m� 5^      $��Öffnen ...��6   $
 Open��6/   m� 5_      $��	Speichern��6   $
 Save��6.   m� 5`      $��Speichern als ...��6   $ SaveAs��66   �� 5a      6   m� 5b      $��Drucken��6   $ Print��6,   6>   m� 5c      $��Beenden��6   $ Close��6,    m� InputGestureText$ Strg+Q��6<   5d      m� 5e      -   5        $��
Bearbeiten��5e      .@�5f      m� $��Kopieren��6   $
 Copy��6-   m� 5g      $��	Einfügen��6   $ Paste��6-   m� 5h      $��Ausschneiden��6   $	 Cut��61   �� 5i      6   m� 5j      $��Suchen��6   # Z 6+   $ Strg+F��6Y   6u   5k      m� 5l      -   5        $��Format��5l      .@�5m      m� $
��fett��6   $  EditingCommands.ToggleBold��6b   $ Strg+B��6�    m� CommandTarget 6)   ��  �� ElementName$ sWriterRichTextBox��6�   m� 5n      $��kursiv��6   $" EditingCommands.ToggleItalic��6d   $ Strg+I��6�    6+   �� $ sWriterRichTextBox��6�   m� 5o      $��unterstrichen��6   $% EditingCommands.ToggleUnderline��6k   $ Strg+U��6�    62   �� $ sWriterRichTextBox��6�   m� 5p      $��Schrift größer��6   $&  EditingCommands.IncreaseFontSize��6l   $ Strg+´��6�    63   �� $ sWriterRichTextBox��6�   m� 5q      $��Schrift kleiner��6   $&  EditingCommands.DecreaseFontSize��6m   $ Strg+ß��6�    64   �� $ sWriterRichTextBox��6�   �� 5r      m� 5s      $��linksbündig��6   $ EditingCommands.AlignLeft��6i   $ Strg+L��6�    60   �� $ sWriterRichTextBox��6�   m� 5t      $��rechstbündig��6   $  EditingCommands.AlignRight��6j   $ Strg+R��6�    61   �� $ sWriterRichTextBox��6�   m� 5u      $��	zentriert��6   $! EditingCommands.AlignCenter��6g   $ Strg+E��6�    6.   �� $ sWriterRichTextBox��6�   m� 5v      $��	Blocksatz��6   $" EditingCommands.AlignJustify��6g   $ Strg+J��6�    6.   �� $ sWriterRichTextBox��6�   �� 5w      m� 5x      $��Einzug vergrößern��6   $) #EditingCommands.IncreaseIndentation��6o   $ Strg+T��6�    66   �� $ sWriterRichTextBox��6�   m� 5y      $��Einzug verringern��6   $) #EditingCommands.DecreaseIndentation��6o   $ Strg+Shift+T��6�    66   �� $ sWriterRichTextBox��6�   �� 5z      m� 5{      $��Aufzählung��6   $# EditingCommands.ToggleBullets��6h   $ Strg+Shift+L��6�    6/   �� $ sWriterRichTextBox��6�   m� 5|      $��Numerierung��6   $% EditingCommands.ToggleNumbering��6i   $ Strg+Shift+N��6�    60   �� $ sWriterRichTextBox��6�   5}      m� 5~      -   5        $��Extras��5~      .@�5      m� $��Einstellungen��6   # Z	 62   5�      m� 5�      -   5        $��Hilfe��5�      .@�5�      m� $��Hilfethemen��6   # Z 60   m� 5�      $��Über SWriter��6   # Z
 61   5�      5�      �� 5�   
   $��Hidden=�6   $��Hidden=�5�      $��1��5�      $��0��5�      $��5,0,5,0q�5�      .��5�      �� -   5        $��sWriterRichTextBox��5�      $��10,10,10,10q�5�       �� AutoWordSelection .   �� 
AcceptsTab . $��10q�5�      $	��100��5�      $	��100��5�       �� 
IsReadOnly .  $��15(�5�       �� 	IsEnabled .   �� SpellingReform$ PreAndPostreform=�5�        � UseLayoutRounding  . 
�������$��0,0,0,0q�5�      $��Stretch=�5�      $��Center=�5�      $	��830��6;   ! �� 	UndoLimit$! 30��5�       5�      ��%�  '/�       'J� *    �� 5�      " /�6    .�5�      ��  �@/�
LineHeight$ 20��67   �� 5�       �@/�IsHyphenationEnabled .  6A   �� 5�       �@/�
PageHeight$ 29.7 cm��67   �� 5�       �@/�	PageWidth$ 21.0 cm��66   �� 5�       �@/�PagePadding$ 15,10,15,10q�68   5�      �� 5�      " J�6    .�5�      ��  �@��Margin$ 10q�67   5�      5�      . �5�      /� .T�5�      J� .*�6$   .,Lösch mich und gib hier deinen Text ein ...6Q   5�      5�      5�      �� 5�   
   -   5        $��indicatorWrapper��5�      " +� IsFocusScope" . # �� Orientation$# 
Horizontal=�5�      $��2��5�      $��0��6"   $��Center=�5�      $��Center=�5�      .+�5�      �� -   5        $��searchBoxBorder��5�      $��1q�6,   $ �� Padding$$ 8,0,8,0q�6@   $��2,2,2,2q�6R   
�������% �� CornerRadius$% 5��6w   ._�5�      �� -   5        $��searchTextBox��5�      $��0q�5�      $��23��5�      $	��120��5�      {�. $y�	Collapsed=�5�      
���   �& �� MaxLines$& 1��5�      ' � ToolTip$#' Hier den Suchbegriff eingeben��5�       . 
�������.��5�   :   ( � Triggers( 5�      :� 5�      ) :� RoutedEvent$) TextBox.GotKeyboardFocus��6'   .[�5�      �� .��5�   "   �� .��5�   &   �� -   5        * g�Name$* 	ColorGlow��5�   ^   + �� TargetProperty$2+ ,(Border.BorderBrush).(SolidColorBrush.Color)�5�   5   , �� 
TargetName$, searchBoxBorder��5�   5   - g� AutoReverse- . . g� Duration$. 0:0:1D�6�   	/ �� To$/ WhiteX�6�   5�   '   t� 5�   &   $+ BorderThickness�69   $, searchBoxBorder��5�   6   0 t� From$0 1X�6^   	1 t� To$1 3X�6g   - . $. 0:0:1D�6�   5�   '   5�   #   5�      5�      .��5�   (   5�      5�      �� 5�      -   5        $��changeIndicatorBorder��5�      $��1q�62   $$ 8,0,8,0q�6F   $��2,2,2,2q�6X   
�������$% 5��6}   ._�5�      �� -   5        $��changeIndicator��5�      2 �� Padding$2 1q�5�      
�������$6' 0Zeigt an dass der Text gespeichert werden sollte��5�      S6W   5�      �� 5�      -   5        $��zoomIndicatorBorder��5�      $��1q�60   $$ 8,0,8,0q�6D   $��2,2,2,2q�6V   
�������$% 5��6{   ._�5�      �� -   5        $��zoomIndicator��5�      $2 1q�5�      
�������$' Zoomfaktor des Textes��5�      100%6>   5�      �� 5�      -   5        $��wordCountIndicatorBorder��5�      $��1q�65   $$ 8,0,8,0q�6I   $��2,2,2,2q�6[   
�������$% 5��6�   ._�5�      �� -   5        $��wordCountIndicator��5�      $2 1q�5�      
�������$ ' Anzahl der Wörter im Text��5�      06@   5�      �� 5�      -    5        $��overwriteIndicatorBorder��5�      $��1q�65   $$ 8,0,8,0q�6I   $��2,2,2,2q�6[   
�������$% 5��6�   ._�5�      �� -!   5        $��overwriteIndicator��5�      $2 1q�5�      
�������$9' 3Zeigt an ob der Überschreibmodus eingeschaltet ist��5�      Ü6Y   5�      �� 5�      -"   5        $��clockIndicatorBorder��5�      $��1q�61   $$ 8,0,8,0q�6E   $��2,2,2,2q�6W   
�������$% 5��6|   ._�5�      �� -#   5        $��clockIndicator��5�      $2 1q�5�      
�������$' Die Uhrzeit��5�      61   5�      5�      5      5     