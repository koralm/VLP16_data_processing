   M S B A M L   `   `   `  ����Q  MLidar_processing_VLP16, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Lidar_processing_VLP16Lidar_processing_VLP16  5   	   '    !Lidar_processing_VLP16.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Lidar_processing_VLP16     5   	   b ^SciChart.Drawing.DirectX, Version=4.2.2.9777, Culture=neutral, PublicKeyToken=b55dd9efe817e823] YSciChart.Charting3D, Version=4.2.2.9777, Culture=neutral, PublicKeyToken=b55dd9efe817e8237s3D+http://schemas.abtsoftware.co.uk/scichart3D   5   	   a ]SciChart.Examples.ExternalDependencies, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullDext:http://schemas.abtsoftware.co.uk/scichart/exampleExternals  5   	   -   5          � Title$  
MainWindow��5   	   $	��768��6   $
��1240��6)   .��5      �  � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5         ��Name$ textBox��5      $
��Left=�6#   $��23��6>   $��
514,26,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $��TextBox��6r   $	��Top=�6�   $	��120��6�   �� 5   
   -   5         ��Name$ button��5      $��Button��6!   $
��Left=�62   $��
544,89,0,0q�6M   $	��Top=�6a   $��75��6y   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGrid��5      $
��Left=�6%   $��	29,10,0,0q�6@   $	��Top=�6S   $	��322��6k   $	��444��6x   �� 5   
   -   5        $ button1��5      $��Button��6"   $
��Left=�63   $��537,142,0,0q�6N   $	��Top=�6c   $��75��6{   6�   � 5   
   $
��Left=�6   $	��517��6*   $��537,196,0,0q�67   $	��Top=�6L   $	��662��6d    5      �� 5      $
��Auto �6#   �� 5      $��* �6#   .+�5      o  iSciChart.Examples.ExternalDependencies.Controls.SciChart3DInteractionToolbar.SciChart3DInteractionToolbar     TargetSurface 6/   ��  �� ElementName$ sciChart��   ExtraContent. 5      C  =SciChart.Examples.ExternalDependencies.Common.FlyoutSeparator  ���#4446&   d� 5      -   5        	 d�Name$	 StartButton��5      64   d� 5      -   5        $	 PauseButton��5      64   d� 5       -   5        $	 ResetButton��5       64   5"      +  %SciChart.Charting3D.SciChart3DSurface  5$      -	   5        
  Name$
 sciChart��5$   $   $��1��66      WorldDimensions6  0SciChart.Charting3D.StringToVector3TypeConverter$ 200,100,200 6F   $��0q�6d      Camera 5&      "  SciChart.Charting3D.Camera3D  5'         ZoomToFitOnAttach . 6#      RenderableSeries 5+      D  >SciChart.Charting3D.RenderableSeries.ScatterRenderableSeries3D  -
   5          Name$ ScatterRenderableSeries3D��5+   4   A  ;SciChart.Charting3D.RenderableSeries.BaseRenderableSeries3D   PointMarker 5,      9	  3SciChart.Charting3D.PointMarkers.PixelPointMarker3D	  5-      8
  2SciChart.Charting3D.PointMarkers.BasePointMarker3D 
  Fill$ 	#77ADFF2F��65   5/         XAxis 53      ,  &SciChart.Charting3D.Axis.NumericAxis3D  54      [ WSciChart.Charting, Version=4.2.2.9777, Culture=neutral, PublicKeyToken=b55dd9efe817e823-  'SciChart.Charting.Visuals.Axes.AxisCore   GrowByK  ESciChart.Charting.Common.Databinding.StringToDoubleRangeTypeConverter$ 0.1, 0.1 6(      	AutoRange$ Always=�6:      YAxis 57        58      $ 0.1, 0.1 6(   $ Always=�6:      ZAxis 5;        5<      $ 0.1, 0.1 6(   $ Always=�6:   5>      5?      5A      5B      