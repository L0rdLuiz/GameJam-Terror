[gd_scene load_steps=2 format=3]

[ext_resource type="Script" path="res://Scenes/MinigamePinceis/MinigamePinceis.cs" id="1_script"]

[node name="MinigamePinceis" type="Node2D"]
script = ExtResource("1_script")

[node name="Fundo" type="ColorRect" parent="."]
mouse_filter = 2
offset_right = 1152.0
offset_bottom = 648.0
color = Color(0.18, 0.18, 0.18, 1)

[node name="PinceisBaguncados" type="Sprite2D" parent="."]
position = Vector2(576, 324)

[node name="PinceisArrumados" type="Sprite2D" parent="."]
position = Vector2(576, 324)

[node name="ArrumarPinceis" type="Button" parent="."]
offset_left = 436.0
offset_top = 540.0
offset_right = 716.0
offset_bottom = 610.0
text = "Arrumar"