   M S B A M L   `   `   `  ����   SWriter    SWriter.SettingsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  SWriter Einstellungen�� � WindowStartupLocation$ CenterOwner=� � SizeToContent$ WidthAndHeight=����	#FFF0F0F0 � Icon$( "/SWriter;component/Application.ico��.���  � RowDefinitions �� $
��Auto ��� $
��Auto �.+��� -   $��settingsTabControl��$��0��$
��Left=�$��5,5,5,5q�$��5,5,5,5q�$	��Top=�.@��� -   $��visualiyationTabItem��$��Darstellung��.��� $	��359��$	��491�� � ColumnDefinitions �� $	��95* ��� $
��258* ��� $
��135* � �� $	��36* ��� $
��129* ��� $
��158* ��� $	��36* �.+��� -   $��textColorLabel��$��
Textfarbe:��$��Right=�$	��0,6q�$��Center=��� -   $��schriftartLabel��$��Schriftart:��$��Right=�$��	0,96,0,10q�$��Center=�$��1���� $��1��$��1��$��2��.+��� $��Center=�$��5q�Schrifttyp:�� -   $��fontFamilyComboBox��$��23��$
��Left=�$��5q�$	��248�� �� Text$  ���� $��Center=�$��5q�
Größe:�� -   $��fontSizeTextBox��$��5q�$	��100��$��23��$
��Left=��� -   $��textColorTextBox��$��5q�$��1��$	��300��$
��Left=�$��2��$��2��$��Beispieltext���� -	   $��initailZoomLabel��$��Start-Zoomfaktor:��$��Right=�$	��0,5q�$��Center=�$��3���� -
   $��initialZoomTextBox��$��5q�$��3��$��1��$��50��$
��Left=���  �� Orientation$ 
Horizontal=�$��1��.+��� -   $��pickTextColorButton��$��Zeichen��$��23��$
��Left=�$��5,5,0,0q�$	��Top=�$��88���� -   $��pickTextBackgroundBUtton��$��Hintergrund��$��23��$
��Left=�$��5,5,0,0q�$	��Top=�$��83���� -   $��fileopsTabItem��$��Speichern/Laden��.���  �� $	��74* ��� $	��40* � �� $	��10* ��� $	��10* �.+��� -   $��keepTextColorsLabel��$)��#Textfarben beim Speichern behalten:��$��Right=�$	��0,5q�$��Center=��� -   $��autosavePeriodLabel��$��Autosave-Zeitraum:��$��Right=�$	��0,5q�$��Center=�$��1���� -   $��keepColorsCheckBox��$��Center=�$��	5,10,0,10q�$��1��$
��Left=��� -   $��autosavePeriodTextBox��$��1��$��1��$��23��$��40��$��5q�$
��Left=��� -   $��miscTabItem��$��Verschiedenes��.���  �� $	��74* ��� $	��40* � �� $	��10* ��� $	��10* �.+��� -   $��spellCheckerActiveLabel��$!��Rechtschreibprüfung aktiv:��$��Right=�$��5q�$��Center=��� -   $��spellCheckerCheckBox��$��0��$��1��$��Center=�$
��Left=��� -   $��oKButton��$��1��$��5q�$
��Left=� �� 	IsDefault . .��OK�� -   $��cancelButton��$��1��$��5q�$��Right=�	 �� IsCancel	 . .��	Abbruch