   M S B A M L   `   `   `  ���� F  BSWriter, Version=1.0.3.34014, Culture=neutral, PublicKeyToken=null!clr-namespace:SWriterSWriter      SWriter.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     (customclr-namespace:SWriter     -     � Title$  SWriter�� � WindowState$ 	Maximized=� � WindowStyle$
 None=�
���   � � Icon$( "/SWriter;component/Application.ico�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ ApplicationCommands.New��  -   $ ApplicationCommands.Open��  -   $ ApplicationCommands.Close��  -   $ ApplicationCommands.Save��  -   $  ApplicationCommands.SaveAs��  -   $ ApplicationCommands.Print��  -      SWriter.MainWindow   Search# Z   -	      SetBold# Z   -
      	SetItalic# Z   -   	   Settings# Z	   -   
   About# Z
   -      Help# Z  J� InputBindings �� 
 �� Key$ Q�� �� 	Modifiers$ Controlf� �� Command$ ApplicationCommands.Close���� $ S��$ Controlf�$ ApplicationCommands.Save���� $ B��$ Controlf�# Z �� $ I��$ Controlf�# Z �� $ F��$ Controlf�# Z  � 	Resources  �� 
TargetType �� BasedOn �� Property �� Value��%l   'o�       7o�'m� (     7m��� " o�8   .���  � ��$ 14(��� " m�8   .���  � ��$ 14(�.��� -   $��mainGrid��$��Stretch=�$��Stretch=�$��0,5,5,5q� � ColumnDefinitions �� $	��100�� � RowDefinitions �� $��10��$
��Auto ��� $	��250���� $
��Auto �.+�o� -   $��mainMenu��$��0��$��0��.@�m� -   $��Datei��.@�m� $	��Neu�� m� Command$	 New��m� $��Öffnen ...��$
 Open��m� $��	Speichern��$
 Save��m� $��Speichern als ...��$ SaveAs���� m� $��Drucken��$ Print��m� $��Beenden��$ Close�� m� InputGestureText$ Strg+Q��m� -   $��
Bearbeiten��.@�m� $��Kopieren��$
 Copy��m� $��	Einfügen��$ Paste��m� $��Ausschneiden��$	 Cut���� m� $��Suchen��# Z $ Strg+F��m� -   $��Format��.@�m� $
��fett��$  EditingCommands.ToggleBold��$ Strg+B�� m� CommandTarget ��  �� ElementName$ sWriterRichTextBox��m� $��kursiv��$" EditingCommands.ToggleItalic��$ Strg+I�� �� $ sWriterRichTextBox��m� $��unterstrichen��$% EditingCommands.ToggleUnderline��$ Strg+U�� �� $ sWriterRichTextBox��m� $��Schrift größer��$&  EditingCommands.IncreaseFontSize��$ Strg+´�� �� $ sWriterRichTextBox��m� $��Schrift kleiner��$&  EditingCommands.DecreaseFontSize��$ Strg+ß�� �� $ sWriterRichTextBox���� m� $��linksbündig��$ EditingCommands.AlignLeft��$ Strg+L�� �� $ sWriterRichTextBox��m� $��rechstbündig��$  EditingCommands.AlignRight��$ Strg+R�� �� $ sWriterRichTextBox��m� $��	zentriert��$! EditingCommands.AlignCenter��$ Strg+E�� �� $ sWriterRichTextBox��m� $��	Blocksatz��$" EditingCommands.AlignJustify��$ Strg+J�� �� $ sWriterRichTextBox���� m� $��Einzug vergrößern��$) #EditingCommands.IncreaseIndentation��$ Strg+T�� �� $ sWriterRichTextBox��m� $��Einzug verringern��$) #EditingCommands.DecreaseIndentation��$ Strg+Shift+T�� �� $ sWriterRichTextBox���� m� $��Aufzählung��$# EditingCommands.ToggleBullets��$ Strg+Shift+L�� �� $ sWriterRichTextBox��m� $��Numerierung��$% EditingCommands.ToggleNumbering��$ Strg+Shift+N�� �� $ sWriterRichTextBox��m� -   $��Extras��.@�m� $��Einstellungen��# Z	 m� -   $��Hilfe��.@�m� $��Hilfethemen��# Z m� $��Über SWriter��# Z
 �� $��Hidden=�$��Hidden=�$��1��$��0��$��5,0,5,0q�.���� -   $��sWriterRichTextBox��$��10,10,10,10q� �� AutoWordSelection .   �� 
AcceptsTab . $��10q�$	��100��$	��100�� �� 
IsReadOnly .  $��15(� �� 	IsEnabled .   �� SpellingReform$ PreAndPostreform=�  � UseLayoutRounding  . 
�������$��0,0,0,0q�$��Stretch=�$��Center=�$	��830��! �� 	UndoLimit$! 30�� ��%  '/�       'J� �     �� " /�.���  �@/�
LineHeight$ 20����  �@/�IsHyphenationEnabled .  ��  �@/�
PageHeight$ 29.7 cm����  �@/�	PageWidth$ 21.0 cm����  �@/�PagePadding$ 15,10,15,10q��� " J�.���  �@��Margin$ 10q�. �/� .T�J� .*�.,Lösch mich und gib hier deinen Text ein ...�� -   $��indicatorWrapper��" +� IsFocusScope" . # �� Orientation$# 
Horizontal=�$��2��$��0��$��Center=�$��Center=�.+��� -   $��searchBoxBorder��$��1q�$ �� Padding$$ 8,0,8,0q�$��2,2,2,2q�
�������% �� CornerRadius$% 5��._��� -   $��searchTextBox��$��0q�$��23��$	��120��{�. $y�	Collapsed=�
���   �& �� MaxLines$& 1��' � ToolTip$#' Hier den Suchbegriff eingeben�� . 
�������.��( � Triggers( :� ) :� RoutedEvent$) TextBox.GotKeyboardFocus��.[��� .���� .���� -   * g�Name$* 	ColorGlow��+ �� TargetProperty$2+ ,(Border.BorderBrush).(SolidColorBrush.Color)�, �� 
TargetName$, searchBoxBorder��- g� AutoReverse- . . g� Duration$. 0:0:1D�	/ �� To$/ WhiteX�t� $+ BorderThickness�$, searchBoxBorder��0 t� From$0 1X�	1 t� To$1 3X�- . $. 0:0:1D�.���� -   $��changeIndicatorBorder��$��1q�$$ 8,0,8,0q�$��2,2,2,2q�
�������$% 5��._��� -   $��changeIndicator��2 �� Padding$2 1q�
�������$6' 0Zeigt an dass der Text gespeichert werden sollte��S�� -   $��zoomIndicatorBorder��$��1q�$$ 8,0,8,0q�$��2,2,2,2q�
�������$% 5��._��� -   $��zoomIndicator��$2 1q�
�������$' Zoomfaktor des Textes��100%�� -   $��wordCountIndicatorBorder��$��1q�$$ 8,0,8,0q�$��2,2,2,2q�
�������$% 5��._��� -   $��wordCountIndicator��$2 1q�
�������$ ' Anzahl der Wörter im Text��0�� -    $��overwriteIndicatorBorder��$��1q�$$ 8,0,8,0q�$��2,2,2,2q�
�������$% 5��._��� -!   $��overwriteIndicator��$2 1q�
�������$9' 3Zeigt an ob der Überschreibmodus eingeschaltet ist��Ü�� -"   $��clockIndicatorBorder��$��1q�$$ 8,0,8,0q�$��2,2,2,2q�
�������$% 5��._��� -#   $��clockIndicator��$2 1q�
�������$' Die Uhrzeit��