   M S B A M L   `   `   `  ����  TrafficSimulation&     TrafficSimulation.InterfaceStart   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5       �� 
TargetType   StartSchermButtons �� Property �� Value �� 
TargetType ��Name �� CornerRadius �� Triggers P� Property	 P� Value
 �� 
TargetName��%�  &	        �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 1,1,1,1q�6'   �� 5       � ��
 � � �6+   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ BorderStyle��6!   
�������
���z���$��3q�5   !   $ 5,5,5,5��5   !   ._�5      �� #��z��6/   #��z��5   /   #��z��5   /   }�. 5!      5"       5#      P� 5$       �@J�IsMouseOver	 . . �5%   "   �� $
 BorderStyle��6)    � ��
 �����6X   �� 5&   "    � ��
 �   �6?   �� 5'   "    �@�Cursor$
 Hand��6;   5(      5*      5,      5-      5/      .��51      � -   5         �Name$ Buttons��51      
��� � � � RowDefinitions 52   
   �� 53      $��* �6   �� 54      $��2* �6   �� 55      $��* �6    � ColumnDefinitions 58   
   �� 59      $��* �6   �� 5:      $��2* �6   �� 5;      $��* �6   .+�5>   
   �� -   5         ��Name$	 weg��5>      $��0��5?      $��0��5@      $��3��5A      $��3��5B      $
��Left=�5C      
�������
�������$	��550��5F      z�5G      �� 5H       �� AngleX$ 36Q�6    5J      �� 5L   
   -   5        $ strepen��5L      $��0��5M      $��0��5N      $��3��5O      $��3��5P      
�������
�������$��15��5S       �� StrokeDashArray$	 3,5R�5T      $
��Left=�5U      $��10��5V      z�5W      �� 5X      $ 35Q�6    5Z      � 5\   
   $��1��6   $��1��6    5^      �� 5_      $��* �6    �� 5`      $��* �6    �� 5a      $��* �6    �� 5b      $��* �6    �� 5c      $��* �6    �� 5d      $��* �6    �� 5e      $��* �6     5h      �� 5i      $��* �6#   .+�5l      �� -   5         ��Name$ resume��5l      $��1��5m      $��0��5n      #��[  5o      $
��Left=�5q      $��Resume��5r      �� 5t      -   5        $ nieuw��5t      $��2��5u      $��0��5v      #��[  5w      $
��Left=�5y      $	��New��5z      �� 5|      -   5        $
 open��5|      $��3��5}      $��0��5~      #��[  5      $��
200,10,5,5q�5�      $
��Left=�5�      $
��Open��5�      �� 5�      -   5        $ howTo��5�      #��[  5�      $��4��5�      $��0��5�      $
��Left=�5�      $��
300,10,5,5q�5�      $��How to��5�      �� 5�      -   5        $ about��5�      #��[  5�      $��5��5�      $��0��5�      $
��Left=�5�      $��
350,10,5,5q�5�      $��About��5�      �� 5�      -	   5        $
 exit��5�      #��[  5�      $��6��5�      $��0��5�      $
��Left=�5�      $��	400,5,5,5q�5�      $
��Exit��5�      5�      �� 5�   
   $��0��6   $��2��5�      $��1��5�      $��2��5�      $	��300��5�      $	��500��5�      $��Right=�5�      $	��Top=�5�      $��	10,0,0,10q�5�      $%��Resources/TrafficExpertLogo.png��5�      �� 5�   
   -
   5         ��Name$ slogan��5�      $��3��5�      $��2��5�      $��0��5�      $��2��5�      $
��Left=�5�      $��Bottom=�5�      $��Italic$�5�      $��50��5�      $	��500��5�      $��40(�5�      
�������How efficient is your road?5�      �� 5�   
   -   5         ��Name$ stopgo��5�      $��3��5�      $��0��5�      $
��Left=�5�      $��Bottom=�5�      $��1,1,1,1q�5�      $!��Resources\stopandgologo.png��5�      5�      5�      