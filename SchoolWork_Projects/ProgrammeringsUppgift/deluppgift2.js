var canvas = document.createElement("canvas");

var gl = canvas.getContext("webgl");



var shared =
{
	worldMatrix: mat4.create(),
	viewMatrix: mat4.create(),
	projectionMatrix: mat4.create(),
	viewProjectionMatrix: mat4.create(),
	worldViewProjectionMatrix: mat4.create(),
	worldInverseMatrix: mat4.create(),
	billboardMatrix: mat4.create(),
	lightIntensity: 1,
	ambientColor: vec4.create(),
	lightPosition: vec4.create(),
	lightPositionObject: vec4.create(),

	worldViewProjectionMatrixLocation: null,
	lightingEnabledLocation: null,
	lightIntensityLocation: null,
	ambientColorLocation: null,
	lightPositionLocation: null,
	vertexPositionLocation: null,
	vertexTextureCoordinateLocation: null,
	vertexNormalLocation: null,
	
	time: 0,
	previousTime: 0,
	run: true,

	worldMatrixStack: [],

	cameraPosition: vec3.create(),
	cameraRotationX: 0,
	cameraRotationY: 0,
	cameraDistance: 0,
	cameraDistanceDelta: 0,

	sunObject: null,
	sunTexture: null,
	sunFlareObject: null,
	sunFlareTexture: null,

	venusObject: null,
    venusTexture: null,
    venusPosition: vec3.create(),

    earthObject: null,
    earthTexture: null,
    earthCloudObject: null,
    earthCloudTexture: null,
    earthPosition: vec3.create(),

    moonObject: null,
    moonTexture: null,
    moonPosition: vec3.create(),


    marsObject: null,
    marsTexture: null,
    marsPosition: vec3.create(),

    jupiterObject: null,
    jupiterTexture: null,
    jupiterPosition: vec3.create(),

    saturnObject: null,
    saturnTexture: null,
    saturnPosition: vec3.create(),

    saturnRingObject: null,
    saturnRingTexture: null,

    ambientLight: 0,
    difuseLight: 0,
	paused: false
};



function main(context)
{
	gl = context;


	window.addEventListener("keydown", keydown);
	window.addEventListener("keyup", keyup);
	gl.canvas.addEventListener("mousemove", mousemove);

	var program = initializeProgram(vertexShader, fragmentShader);
	if (!program)
	{
		window.removeEventListener("keydown", keydown);
		window.removeEventListener("keyup", keyup);
		gl.canvas.removeEventListener("mousemove", mousemove);
		return;
	}

	gl.useProgram(program);
	shared.worldViewProjectionMatrixLocation = gl.getUniformLocation(program, "u_worldViewProjection");
	shared.lightingEnabledLocation = gl.getUniformLocation(program, "u_lightingEnabled");
	shared.lightIntensityLocation = gl.getUniformLocation(program, "u_lightIntensity");
	shared.ambientColorLocation = gl.getUniformLocation(program, "u_ambientColor");
	shared.lightPositionLocation = gl.getUniformLocation(program, "u_lightPosition");
	shared.vertexPositionLocation = gl.getAttribLocation(program, "a_position");
	shared.vertexTextureCoordinateLocation = gl.getAttribLocation(program, "a_textureCoordinate");
	shared.vertexNormalLocation = gl.getAttribLocation(program, "a_normal");
	gl.enableVertexAttribArray(shared.vertexPositionLocation);
	gl.enableVertexAttribArray(shared.vertexTextureCoordinateLocation);
	gl.enableVertexAttribArray(shared.vertexNormalLocation);

	var aspectRatio = gl.drawingBufferWidth / gl.drawingBufferHeight;
	mat4.perspective(shared.projectionMatrix, Math.PI/4, aspectRatio, 1, 200);

	initializeScene();


	window.requestAnimationFrame(frameCallback);
}



function initializeScene()
{
	shared.cameraDistance = 80;

    //Sets the type of object, size and textures
	shared.sunObject = twgl.primitives.createSphereBufferInfo(gl, 5, 32, 32);
    shared.sunFlareObject = twgl.primitives.createPlaneBufferInfo(gl, 55, 55);
    shared.sunTexture = loadTexture("2k_sun.jpg");
	shared.sunFlareTexture = loadTexture("lensflare.png");

	shared.venusObject = twgl.primitives.createSphereBufferInfo(gl, 3, 32, 32);
	shared.venusTexture = loadTexture("2k_venus_surface.jpg");

	shared.earthObject = twgl.primitives.createSphereBufferInfo(gl, 3, 32, 32);
	shared.earthTexture = loadTexture("2k_earth_daymap.jpg");

	shared.earthCloudObject = twgl.primitives.createSphereBufferInfo(gl, 3.05, 32, 32);
	shared.earthCloudTexture = loadTexture("2k_earth_clouds.jpg");

	shared.moonObject = twgl.primitives.createSphereBufferInfo(gl, 1, 32, 32);
	shared.moonTexture = loadTexture("2k_moon.jpg");

	shared.marsObject = twgl.primitives.createSphereBufferInfo(gl, 2.5, 32, 32);
	shared.marsTexture = loadTexture("2k_mars.jpg");

	shared.jupiterObject = twgl.primitives.createSphereBufferInfo(gl, 5, 32, 32);
	shared.jupiterTexture = loadTexture("2k_jupiter.jpg");

    shared.saturnObject = twgl.primitives.createSphereBufferInfo(gl, 5, 32, 32);
    shared.saturnTexture = loadTexture("2k_saturn.jpg");

    shared.saturnRingObject = twgl.primitives.createDiscBufferInfo(gl, 12, 32, 32, 6);
    shared.saturnRingTexture = loadTexture("2k_saturn.jpg");
}



function frameCallback(time)
{
	var deltaTime = time - shared.previousTime;
	if (!shared.paused) shared.time += deltaTime;
	shared.previousTime = time;

	frame(shared.time * 0.001, deltaTime * 0.001);

	if (shared.run) window.requestAnimationFrame(frameCallback);
}



function keydown(event)
{
	switch (event.key)
	{
		case "p":
			shared.paused = !shared.paused;
			break;
		case "ArrowUp":
			shared.cameraDistanceDelta = -1;
			break;
		case "ArrowDown":
			shared.cameraDistanceDelta = 1;
			break;


	    case "a":
	        shared.ambientLight += 1;
	        if (shared.ambientLight > 3)
	        {
	            shared.ambientLight = 0;
	        }
	        break;

	    case "d":
	        shared.difuseLight += 1;
	        if (shared.difuseLight > 3) {
	            shared.difuseLight = 0;
	        }
	        break;
	}
}



function keyup(event)
{
	switch (event.key)
	{
		case "ArrowUp":
		case "ArrowDown":
			shared.cameraDistanceDelta = 0;
			break;
	}
}



function mousemove(event)
{
	if (event.buttons == 1)
	{
		shared.cameraRotationX += -event.movementY * 0.01;
		shared.cameraRotationY += event.movementX * 0.01;

		var limitAngleX = Math.PI / 3;
		shared.cameraRotationX = Math.max(shared.cameraRotationX, -limitAngleX);
		shared.cameraRotationX = Math.min(shared.cameraRotationX, limitAngleX);
	}
}



function setTransformationAndLighting(lighting)
{
	mat4.multiply(shared.worldViewProjectionMatrix, shared.viewProjectionMatrix, shared.worldMatrix);
	gl.uniformMatrix4fv(shared.worldViewProjectionMatrixLocation, false, shared.worldViewProjectionMatrix);

	gl.uniformMatrix4fv(shared.worldMatrixLocation, false, shared.worldMatrix);

	gl.uniform1i(shared.lightingEnabledLocation, lighting);

	gl.uniform1f(shared.lightIntensityLocation, shared.lightIntensity);

	mat4.invert(shared.worldInverseMatrix, shared.worldMatrix);
	vec4.transformMat4(shared.lightPositionObject, shared.lightPosition, shared.worldInverseMatrix);
	gl.uniform4fv(shared.lightPositionLocation, shared.lightPositionObject);

	gl.uniform4fv(shared.ambientColorLocation, shared.ambientColor);
}



function pushWorldMatrix()
{
	shared.worldMatrixStack.push(mat4.clone(shared.worldMatrix));
}



function popWorldMatrix()
{
	if (shared.worldMatrixStack.length == 0)
	{
		console.log("worldMatrixStack: Can't pop matrix from empty stack"); 
	}

	mat4.copy(shared.worldMatrix, shared.worldMatrixStack.pop());
}

function drawAtmosfear(object)
{
    //Draws the Atmosfear
    gl.enable(gl.BLEND);
    gl.blendFunc(gl.SRC_ALPHA, gl.ONE);
    gl.disable(gl.CULL_FACE);



    gl.bindBuffer(gl.ARRAY_BUFFER, object.attribs.position.buffer);
    gl.vertexAttribPointer(shared.vertexPositionLocation, object.attribs.position.numComponents, object.attribs.position.type, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ARRAY_BUFFER, object.attribs.texcoord.buffer);
    gl.vertexAttribPointer(shared.vertexTextureCoordinateLocation, object.attribs.texcoord.numComponents, object.attribs.texcoord.type, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ARRAY_BUFFER, object.attribs.normal.buffer);
    gl.vertexAttribPointer(shared.vertexNormalLocation, object.attribs.normal.numComponents, object.attribs.normal.type, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, object.indices);
    gl.drawElements(gl.TRIANGLES, object.numElements, object.elementType, 0);



    gl.enable(gl.CULL_FACE);
    gl.disable(gl.BLEND);
}

function drawObject(object)
{
	gl.bindBuffer(gl.ARRAY_BUFFER, object.attribs.position.buffer);
	gl.vertexAttribPointer(shared.vertexPositionLocation, object.attribs.position.numComponents, object.attribs.position.type, gl.FALSE, 0, 0);

	gl.bindBuffer(gl.ARRAY_BUFFER, object.attribs.texcoord.buffer);
	gl.vertexAttribPointer(shared.vertexTextureCoordinateLocation, object.attribs.texcoord.numComponents, object.attribs.texcoord.type, gl.FALSE, 0, 0);

	gl.bindBuffer(gl.ARRAY_BUFFER, object.attribs.normal.buffer);
	gl.vertexAttribPointer(shared.vertexNormalLocation, object.attribs.normal.numComponents, object.attribs.normal.type, gl.FALSE, 0, 0);

	gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, object.indices);
	gl.drawElements(gl.TRIANGLES, object.numElements, object.elementType, 0);
}



function frame(time, deltaTime)
{
	gl.clearColor(0, 0, 0, 1);
	gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

	gl.enable(gl.CULL_FACE);
	gl.enable(gl.DEPTH_TEST);

	shared.cameraDistance += shared.cameraDistanceDelta * deltaTime * 35;
	vec3.set(shared.cameraPosition, 0, 0, -shared.cameraDistance);
	vec3.rotateX(shared.cameraPosition, shared.cameraPosition, vec3.fromValues(0, 0, 0), shared.cameraRotationX);
	vec3.rotateY(shared.cameraPosition, shared.cameraPosition, vec3.fromValues(0, 0, 0), shared.cameraRotationY);
	mat4.lookAt(shared.viewMatrix, shared.cameraPosition, vec3.fromValues(0, 0, 0), vec3.fromValues(0, 1, 0));
	mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

	drawScene(time);

	if (shared.worldMatrixStack.length > 0)
	{
		console.log("worldMatrixStack: Push and pop misalignment"); 
		shared.run = false;
	}
}



function drawScene(time)
{
    //Sets teh light intensity
    if (shared.difuseLight == 0)
    {
        shared.lightIntensity = 1;
    }

    else if (shared.difuseLight == 1) {
        shared.lightIntensity = 2;
    }

    else if (shared.difuseLight == 2) {
        shared.lightIntensity = 4;
    }
    else if (shared.difuseLight == 3) {
        shared.lightIntensity = 8;
    }

    //Sets the ambientcolor
    if (shared.ambientLight == 0) {
        shared.ambientColor = vec4.fromValues(0.3, 0.3, 0.3, 1);
    }
    else if (shared.ambientLight == 1) {
        shared.ambientColor = vec4.fromValues(1, 0, 0, 1);
    }
    else if (shared.ambientLight == 2) {
        shared.ambientColor = vec4.fromValues(0, 1, 0, 1);
    }

    else if (shared.ambientLight == 3) {
        shared.ambientColor = vec4.fromValues(0, 0, 1, 1);
    }

    console.log(shared.difAmbLevelsChanged);

    shared.lightPosition = vec4.fromValues(0, 0, 0, 1);

	var world = shared.worldMatrix;


	mat4.identity(world);


    //Venus
    pushWorldMatrix();

    mat4.rotate(world, world, time * 20 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the sun
    mat4.translate(world, world, vec3.fromValues(10, 0, 0));
    mat4.rotate(world, world, time * 60 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates the planet

	setTransformationAndLighting(true);
	gl.bindTexture(gl.TEXTURE_2D, shared.venusTexture);
    drawObject(shared.venusObject);

    popWorldMatrix();

    drawEarth(time);

    drawMars(time);

    drawJupiter(time);

    drawSaturn(time);

    //mat4.rotate(world, world, time * 30 / 180 * Math.PI, vec3.fromValues(0, 1, 0));
	drawSun(time);
}

function drawEarth(time)
{
    var world = shared.worldMatrix;

    //Earth
    pushWorldMatrix();

    mat4.rotate(world, world, time * 30 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the sun
    mat4.translate(world, world, vec3.fromValues(25, 0, 0));
    mat4.rotate(world, world, time * 40 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates the planet

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.earthTexture);
    drawObject(shared.earthObject);

    //Earth atmosfear
    pushWorldMatrix();

    mat4.rotate(world, world, time * -100 / 180 * Math.PI, vec3.fromValues(0, 1, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.earthCloudTexture);
    drawAtmosfear(shared.earthCloudObject);

    popWorldMatrix();


    //Earth moon
    pushWorldMatrix();

    mat4.rotate(world, world, time * 40 / 180 * Math.PI, vec3.fromValues(1, 0, 1));     //rotates around the planet
    mat4.translate(world, world, vec3.fromValues(5, 0, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.moonTexture);
    drawObject(shared.moonObject);

    popWorldMatrix();

    popWorldMatrix();
}

function drawMars(time)
{
    var world = shared.worldMatrix;

    pushWorldMatrix();

    mat4.rotate(world, world, time * 15 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the sun
    mat4.translate(world, world, vec3.fromValues(30, 0, 0));
    mat4.rotate(world, world, time * 40 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates the planet

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.marsTexture);
    drawObject(shared.marsObject);


    //moon
    pushWorldMatrix();

    mat4.rotate(world, world, time * 30 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the planet
    mat4.translate(world, world, vec3.fromValues(4, 0, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.moonTexture);
    drawObject(shared.moonObject);

    popWorldMatrix();

    //moon
    pushWorldMatrix();

    mat4.rotate(world, world, time * -90 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the planet
    mat4.translate(world, world, vec3.fromValues(6, 0, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.moonTexture);
    drawObject(shared.moonObject);

    popWorldMatrix();


    popWorldMatrix();

}

function drawJupiter(time) {
    var world = shared.worldMatrix;

    pushWorldMatrix();

    mat4.rotate(world, world, time * 10 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the sun
    mat4.translate(world, world, vec3.fromValues(45, 0, 0));
    mat4.rotate(world, world, time * 40 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates the planet

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.jupiterTexture);
    drawObject(shared.jupiterObject);


    //moon
    pushWorldMatrix();

    mat4.rotate(world, world, time * 40 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the planet
    mat4.translate(world, world, vec3.fromValues(6, 0, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.moonTexture);
    drawObject(shared.moonObject);

    popWorldMatrix();

    //moon
    pushWorldMatrix();

    mat4.rotate(world, world, time * -10 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the planet
    mat4.rotate(world, world, time * 50 / 180 * Math.PI, vec3.fromValues(0, 0, 1));     //rotates around the planet
    mat4.translate(world, world, vec3.fromValues(6, 0, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.moonTexture);
    drawObject(shared.moonObject);

    popWorldMatrix();


    //moon
    pushWorldMatrix();

    mat4.rotate(world, world, time * -10 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the planet
    mat4.rotate(world, world, time * 50 / 180 * Math.PI, vec3.fromValues(1, 0, 0));     //rotates around the planet
    mat4.translate(world, world, vec3.fromValues(0, 6, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.moonTexture);
    drawObject(shared.moonObject);

    popWorldMatrix();


    popWorldMatrix();

}

function drawSaturn(time)
{
    var world = shared.worldMatrix;

    pushWorldMatrix();

    mat4.rotate(world, world, time * 5 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates around the sun
    mat4.translate(world, world, vec3.fromValues(60, 0, 0));
    mat4.rotate(world, world, 20 / 180 * Math.PI, vec3.fromValues(1, 0, 0));            //Tilting the planet
    mat4.rotate(world, world, time * 40 / 180 * Math.PI, vec3.fromValues(0, 1, 0));     //rotates the planet
    mat4.scale(world, world, vec3.fromValues(1, 1.5, 1));                               //Scales the planet

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.saturnTexture);
    drawObject(shared.saturnObject);


    //Draws saturns ring
    pushWorldMatrix();

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.saturnRingTexture);
    drawObject(shared.saturnRingObject);

    pushWorldMatrix();

    mat4.rotate(world, world, 180 / 180 * Math.PI, vec3.fromValues(1, 0, 0));

    setTransformationAndLighting(true);
    gl.bindTexture(gl.TEXTURE_2D, shared.saturnRingTexture);
    drawObject(shared.saturnRingObject);

    popWorldMatrix();
    popWorldMatrix();


    popWorldMatrix();
}


function drawSun(time)
{
    var world = shared.worldMatrix;

    pushWorldMatrix();

    mat4.rotate(world, world, time * 30 / 180 * Math.PI, vec3.fromValues(0, 1, 0));

	setTransformationAndLighting(false);
	gl.bindTexture(gl.TEXTURE_2D, shared.sunTexture);
	drawObject(shared.sunObject);

	popWorldMatrix();

	billboardTransformation(shared.billboardMatrix, shared.viewMatrix);
	mat4.rotateX(world, world, Math.PI/2);
	mat4.multiply(world, shared.billboardMatrix, world);

	setTransformationAndLighting(false);
	gl.bindTexture(gl.TEXTURE_2D, shared.sunFlareTexture);
	gl.enable(gl.BLEND);
	gl.blendFunc(gl.SRC_ALPHA, gl.ONE);
	drawObject(shared.sunFlareObject);
	gl.disable(gl.BLEND);
}



var vertexShader =
`
	uniform mat4 u_worldViewProjection;
	uniform bool u_lightingEnabled;
	uniform float u_lightIntensity;
	uniform vec4 u_lightPosition;
	attribute vec4 a_position;
	attribute vec2 a_textureCoordinate;
	attribute vec3 a_normal;
	varying vec2 v_textureCoordinate;
	varying float v_diffuse;

	void main(void)
	{
		v_diffuse = 0.0;
		if (u_lightingEnabled)
		{
			vec3 lightDirection = normalize(u_lightPosition.xyz - a_position.xyz); 
			v_diffuse = max(dot(a_normal, lightDirection), 0.0) * u_lightIntensity;
		}
		v_textureCoordinate = a_textureCoordinate;
		gl_Position = u_worldViewProjection * a_position;
	}
`;



var fragmentShader =
`
	uniform sampler2D texture;
	uniform bool u_lightingEnabled;
	uniform highp vec4 u_ambientColor;
	varying highp vec2 v_textureCoordinate;
	varying highp float v_diffuse;
	precision highp float;

	void main(void)
	{
		vec4 lighting = vec4(1);
		if (u_lightingEnabled)
		{
			lighting = vec4(v_diffuse, v_diffuse, v_diffuse, 1) + u_ambientColor;
		}
		gl_FragColor = texture2D(texture, v_textureCoordinate) * lighting;
	}
`;
