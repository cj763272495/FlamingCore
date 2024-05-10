= Adjust Pivot =

Online documentation available at: https://github.com/yasirkula/UnityAdjustPivot
E-mail: yasirkula@gmail.com

1. ABOUT
= �������ĵ� =

�����ĵ�����������ַ�ҵ���https://github.com/yasirkula/UnityAdjustPivot
�����ʼ���yasirkula@gmail.com

1. ����
�ù��߰������ڲ������յĸ�������Ϊ���ĵ������¸��Ķ�������ĵ㡣���������͵����ĵ������

a. �������û�����񣨾�����˵��MeshFilter������ű�ֻ����Ӧ�ظ����Ӷ����λ�ú���ת
b. ���������������ű����ȴ��������ʵ����ͨ���޸��䶥�㡢���ߺ�������������������ĵ㣬�����Ӧ�ظ����Ӷ����λ�ú���ת

2. ��β���
Ҫ���Ķ�������ĵ㣬�봴��һ���յ�����Ϸ���󲢽����ƶ�����������ĵ�λ�á�Ȼ��ͨ��"Window-Adjust Pivot"�˵���"Adjust Pivot"���ڣ�
������"Move X's pivot here"��ť������������ĵ��ƶ�����λ�á�֮����԰�ȫ��ɾ���յ��Ӷ���

��ע�⣬��������������ѡ��b����Ҫ������Ӧ����Ԥ�Ƽ��������뽫ʵ���������񱣴浽��Ŀ�С����򣬸���Դ�������л��ڳ����У������޷���Ԥ�Ƽ���ʹ�á���������ѡ�

- �����񱣴�Ϊ��Դ��.asset��
- �����񱣴�ΪOBJ�ļ���.obj��

֮�������԰�ȫ�ؽ�����Ӧ����Ԥ�Ƽ���
This tool helps you change the pivot point of an object without having to create an empty parent object as the pivot point. There are two types of pivot adjustments:

a. If the object does not have a mesh (MeshFilter, to be precise), then the script simply changes the positions and rotations of child objects accordingly
b. If the object does have a mesh, then the script first creates an instance of the mesh, adjusts the mesh's pivot point by altering its vertices, normals and tangents, and finally changes the positions and rotations of child objects accordingly

2. HOW TO
To change an object's pivot point, create an empty child GameObject and move it to the desired pivot position. Then, open the Adjust Pivot window via the Window-Adjust Pivot menu and press the "Move X's pivot here" button to move the parent object's pivot there. It is safe to delete the empty child object afterwards.

Note that if the object has a mesh (option b), to apply the changes to the prefab, you have to save the instantiated mesh to your project. Otherwise, the asset will be serialized in the scene and won't be available to the prefab. You have two options there:

- save the mesh as asset (.asset)
- save the mesh as OBJ (.obj)

Afterwards, you can safely apply your changes to the prefab.