   M S B A M L   `   `   `  ����I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:WpfTaskManager.ConvertersWpfTaskManager.Converters  5   	   C&clr-namespace:TaskManager.LocalizationTaskManager.Localization  5   	       WpfTaskManager.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   >
converters'clr-namespace:WpfTaskManager.Converters     5   	   ?localization&clr-namespace:TaskManager.Localization     5   	     � Title$  WPF Task Manager��5   	   $	��800��6"   $
��1366��6/   ���#ccffcc$��Colbert)�5   	    � 	Resources 5	      5   /WpfTaskManager.Converters.BoolToStatusConverter   BoolToStatusConverter �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value&   System.Windows.Controls.DataGrid*  $System.Windows.Controls.DataGridCell=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%W  &	        '��      '��     '  �    '  �      5
   
   6+   �� 5   
   " ��6   .�5      ��  � �� �#00cc666+   �� 5       � ��
 �   �6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$
 10,5q�6(   �� 5       � ��$ 16(�6)   �� 5       � ��$ 
10,30,10,0q�6'   �� 5       � ��$ 2q�60   �� 5       � ��$ 30��6'   �� 5       � �� �#0099336,   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 10��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5!       5"      P� 5#       �@J�IsMouseOver$
	 True��61   . �5$      ��  � ��
 �����63   �� 5%       � ��
 ��i��64   5&      5(      �� 5*   
   " ��6   .�5+      ��  � ��$
 Bold"�6+   �� 5,       � ��$ 16(�6)   �� 5-       � ��$ 
10,15,10,0q�6'   5.      �� 50   
   "  6   .�51      ��  � �� �#ccffcc6+   �� 52       � ��
 �   �6+   �� 53       � ��$
 Bold"�6+   �� 54       � ��$ 16(�6)   �� 55       � �� �#0099336,   �� 56       � ��$ 2q�60   �� 57       �@ RowBackground �#ccffcc6.   �� 58        �@ AlternatingRowBackground �#b3ffb369   �� 59       �@ 	CellStyle 5:      �� 5;      "  6   .�5<      ��  � ��$
 10,5q�64   �� 5=       � ��$	 0.5q�6<   �� 5>       � �� �#00993368   5?      5A      5B      �� 5D   
   "  6   .�5E      ��  � �� �#00cc666+   �� 5F       � ��
 �   �6+   �� 5G       � ��$
 Bold"�6+   �� 5H       � ��$ 16(�6)   �� 5I       � �� �#0099336,   �� 5J       � ��$ 2q�60   �� 5K       � ��$
 10,5q�6(   5L      5M      .��5N      � 
 � ColumnDefinitions
 5O   
   �� 5P      $��2* �6   �� 5Q      $��* �6    � RowDefinitions 5S   
   �� 5T      $
��auto �6   �� 5U      $��1* �6   �� 5V      $��2* �6   .+�5Z   
   � $��0��6   
 5[      �� 5\      $��* �6#   �� 5]      $
��Auto �6#   .+�5a      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��0,10,0,5q�6O   .+�5b      ��   
SearchText#���  6   $��Center=�5c      $
��Bold"�63   $��16(�6E   $��0,0,5,0q�6S   �� 5d      -   5        $	��100��5d      $��30��6&   ��5e      �� *Input+ �� UpdateSourceTrigger$ PropertyChanged=�5f      5g      �� 5j      $ 
Horizontal=�6   $��1��62   $��Center=�6B   $��	0,10,10,5q�6_   .+�5k      ��   LanguageText#���  6   $��Center=�5l      $
��Bold"�63   $��16(�6E   $��0,0,5,0q�6S   �� 5m      $	��100��6   $��30��6'   $��0,0,0,5q�5n      $��Center=�5o      $��Center=�6?   ��5p      �� *AvailableLanguages+ �� SelectedItem 5q      �� *SelectedLanguage+5r      5s        5v   
   -   5        $��	TasksGrid��5v      $��1��5w         AutoGenerateColumns$ False��5x         
IsReadOnly$
 True��6/   ��5v   $   �� *	Records+   Columns 5y      0  *System.Windows.Controls.DataGridTextColumn  5z      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5{        	TitleText# �  6%   $ * 6d    6J   �� *Title+6d     5|        DescriptionText# �  6%   $	 200 6p    6P   �� *Description+6p     5}        
StatusText# �  6%   $	 100 6�    6K   �� *IsCompleted+ �� 	Converter# [  6�     5~        CategoryText# �  6%   $	 100 6j    6M   �� *
Category+6j     5        PriorityText# �  6%   $	 100 6j    6M   �� *
Priority+6j     5�        DueDateText# �  6%   $	 100 6�    6L   �� *	DueDate+ �� StringFormat$ 
dd-MM-yyyy��6�   5�      �� 5�   
   $��2��6   $
��Auto=�6$   $
��Auto=�6I   .��5�      �� $ 
Horizontal=�6   .+�5�      �� -   5         ��Name$ MainFieldsPanel��5�      $ Vertical=�6^   y�66   �� *FieldsVisibility+.+�5�      A�  A� Columns$ 2��6"   .+�5�      �� #���  6    �� 5�      $	��200��69   ��6"   �� *Title+69   �� 5�      #���  6    �� 5�      $	��200��6?   ��6"   �� *Description+6?   �� 5�      #���  6    �� 5�      $	��200��6<   ��6"   �� *
Category+6<   �� 5�      .��5�      �� #���  6(   5�      �� 5�      $��Center=�5�      $��Center=�6A   $	��200��5�   g   $��	0,10,0,10q�5�      ��5�      �� *PriorityOptions+ 6E   �� *
Priority+5�      �� 5�      .��5�      �� #���  6(   5�      (	  "System.Windows.Controls.DatePicker	  5�      $	��200��6F   $��	0,10,0,10q�6R    	  SelectedDate 6%   �� *	DueDate+6R   5�      �� 5�      -   5         	 AddText#��� 	 5�      y�5�      �� *AddConfirmVisibility+5�      �� 5�      -   5         
 EditText#��� 
 5�      y�5�      �� *EditConfirmVisibility+5�      5�      � 5�      $��20,0,0,0q�6   $	��Top=�6)   $	��300��6A   $	��200��6M   .+�5�      �� $��2q�6   ���#009933$ 10��6G   ���#ccffcc �� Padding$ 10q�5�      y�5�      �� *DeleteVisibility+._�5�      �� .+�5�      ��   EnterDeleteIdText#���  6(   
���   �$��5q�5�   ;   $
��Bold"�6F   �� 5�      -   5        ���#ccffcc
���   �$��5q�5�   &   ��5�   &   �� *
DeleteId+$ PropertyChanged=�5�   &   �� 5�      -   5          
DeleteText#���  5�   %   $��5q�5�   %   5�   %   5�      5�      �� 5�      $��2q�6   ���#009933$ 10��6G   ���#ccffcc$ 10q�5�      y�5�      �� *ToggleVisibility+._�5�      �� .+�5�      ��   EnterToggleIdText#���  6(   
���   �$��5q�5�   ;   $
��Bold"�6F   �� 5�      -   5        ���#ccffcc
���   �$��5q�5�   &   ��5�   &   �� *
ToggleId+$ PropertyChanged=�5�   &   �� 5�      -	   5          ChangeStatusText#���  5�   %   $��5q�5�   %   5�   %   5�      5�      �� 5�      $��2q�6   ���#009933$ 10��6G   ���#ccffcc$ 10q�5�      y�5�      �� *EditVisibility+._�5�      �� .+�5�      ��   EnterEditIdText#���  6(   
���   �$��5q�5�   ;   $
��Bold"�6F   �� 5�      -
   5        ���#ccffcc
���   �$��5q�5�   &   ��5�   &   �� *EditId+$ PropertyChanged=�5�   &   �� 5�      -   5          SearchByIdText#���  5�   %   $��5q�6i   5�      5�      �� 5�      $��2q�6   ���#009933$ 10��6G   ���#ccffcc$ 10q�5�      y�5�      �� *CategoryFilterVisibility+._�5�      �� .+�5�      ��   SearchByCategoryText#���  6(   
���   �$��5q�5�   ;   $
��Bold"�6F   �� 5�      -   5        ���#ccffcc
���   �$��5q�5�   &   ��5�   &   �� *CategoryFilterText+$ PropertyChanged=�5�   &   �� 5�      -   5        #���  5�   %   $��5q�5�   %   5�   %   5�      5�      5�      5�      5�      � 5�   
   $��0��6   $��4��6   $��1��6-   .+�5�      �� .+�5�      �� -   5          AllTasksText#���  5�      6B   �� 5�      -   5          AddTaskText#���  5�      6A   �� 5�      -   5          EditTaskText#���  5�      6B   �� 5      -   5          DeleteTaskText#���  5      6D   �� 5     -   5          ToggleTaskText#���  5     6D   �� 5     -   5          ListIncompleteText#���  5     6H   �� 5     -   5          ListByPriorityText#���  5     6H   �� 5     -   5          FilterByCategoryText#���  5     6J   �� 5     -   5          ExitText#���  5     6>   5     5     �� 5
  
   $��TaskManager��6   ���#004d00$
��Bold"�5     $��Impact)�5     $��70(�5     $��Center=�5     $	��Top=�5     $��	0,340,0,0q�5     $��1��5     $��2��5     $	��110��5     $	��417��5     5     5     