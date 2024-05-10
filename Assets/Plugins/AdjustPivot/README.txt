= Adjust Pivot =

Online documentation available at: https://github.com/yasirkula/UnityAdjustPivot
E-mail: yasirkula@gmail.com

1. ABOUT
= 调整中心点 =

在线文档可在以下网址找到：https://github.com/yasirkula/UnityAdjustPivot
电子邮件：yasirkula@gmail.com

1. 关于
该工具帮助您在不创建空的父对象作为中心点的情况下更改对象的中心点。有两种类型的中心点调整：

a. 如果对象没有网格（具体来说是MeshFilter），则脚本只会相应地更改子对象的位置和旋转
b. 如果对象有网格，则脚本首先创建网格的实例，通过修改其顶点、法线和切线来调整网格的中心点，最后相应地更改子对象的位置和旋转

2. 如何操作
要更改对象的中心点，请创建一个空的子游戏对象并将其移动到所需的中心点位置。然后，通过"Window-Adjust Pivot"菜单打开"Adjust Pivot"窗口，
并按下"Move X's pivot here"按钮将父对象的中心点移动到该位置。之后可以安全地删除空的子对象。

请注意，如果对象具有网格（选项b），要将更改应用于预制件，您必须将实例化的网格保存到项目中。否则，该资源将被序列化在场景中，并且无法在预制件中使用。您有两个选项：

- 将网格保存为资源（.asset）
- 将网格保存为OBJ文件（.obj）

之后，您可以安全地将更改应用于预制件。
This tool helps you change the pivot point of an object without having to create an empty parent object as the pivot point. There are two types of pivot adjustments:

a. If the object does not have a mesh (MeshFilter, to be precise), then the script simply changes the positions and rotations of child objects accordingly
b. If the object does have a mesh, then the script first creates an instance of the mesh, adjusts the mesh's pivot point by altering its vertices, normals and tangents, and finally changes the positions and rotations of child objects accordingly

2. HOW TO
To change an object's pivot point, create an empty child GameObject and move it to the desired pivot position. Then, open the Adjust Pivot window via the Window-Adjust Pivot menu and press the "Move X's pivot here" button to move the parent object's pivot there. It is safe to delete the empty child object afterwards.

Note that if the object has a mesh (option b), to apply the changes to the prefab, you have to save the instantiated mesh to your project. Otherwise, the asset will be serialized in the scene and won't be available to the prefab. You have two options there:

- save the mesh as asset (.asset)
- save the mesh as OBJ (.obj)

Afterwards, you can safely apply your changes to the prefab.