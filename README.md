# 2D Raytracer

![An example render](https://i.postimg.cc/v8xJYgWD/glass-box-denoised.png)

This is a small project that implements 2D ray tracing.

## Usage

Command line: `render <scale> <samples> <scene file> <output file>`
```
scale: the size of the scene will be scaled by this value, resulting in a higher or lower resolution image

samples: number of samples to use per pixel

scene file: location of the scene to be rendered

output file: desired location for the output image
```


## Scene Files

Scenes should be stored in a plain text format. The first line of the file should be the dimensions of the scene.

The rest of the file defines the list of objects in the scene. Objects have a shape and a material, specificed independently.

```
Object format:    <shape>|<material>
Shape format:     <shape_name>,<param1>,<param2>,...
Material format:  <material_name>,<param1>,<param2>,...
```

### Shapes

```
Circle:   x_center, y_center, radius
Box:      x_left,   y_bottom, x_right,      y_top
Torus:    x_center, y_center, inner_radius, outer_radius
```

### Materials

```
Diffuse:  roughness
Light:    r,  g,  b                 //These values are not bounded; greater values will results in a brighter light
Glass:    ior,  r,  g,  b
Rainbow:  intensity,  band_length   //Rainbow lights
```

### Example Scene

```
200,200
torus,100,100,50,80|rainbow,1,2
box,0,0,10,30|glass,1.3,0,0,0
circle,100,180,10|light,1.5,1.5,1.5
```
