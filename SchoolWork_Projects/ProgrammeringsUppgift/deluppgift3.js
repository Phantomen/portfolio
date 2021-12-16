var gl;



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

	camera: null,
    cameraPosition: vec3.create(),
    cameraRotationX: 0,
    cameraRotationY: 0,
    cameraRotationZ: 0,
    cameraDistanceDeltaZ: 0,
    cameraDistanceDeltaX: 0,
    cameraRelativeView: true,

    fourViews: true,

    cameraLight: false,

    planeObject: null,
	coneObject: null,
	sphereObject: null,
	cubeObject1: null,
	cubeObject2: null,

	chessboardTexture: null,
	whiteTexture: null,

	paused: false,
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
    shared.camera = createCamera(0, 0, -85);

	shared.planeObject = twgl.primitives.createPlaneBufferInfo(gl, 90, 90, 16, 16);
	shared.coneObject = twgl.primitives.createTruncatedConeBufferInfo(gl, 8, 2, 40, 32, 32);
	shared.sphereObject = twgl.primitives.createSphereBufferInfo(gl, 1.5, 32, 32);
	shared.cubeObject1 = twgl.primitives.createCubeBufferInfo(gl, 10);
	shared.cubeObject2 = twgl.primitives.createCubeBufferInfo(gl, 20);

	shared.chessboardTexture = loadTexture("chessboard.png");
	shared.whiteTexture = loadTexture("white.png");
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

            //sets what it should move and relative
	    case "w":
	        shared.cameraDistanceDeltaZ = 1;
	        shared.cameraRelativeView = true;
	        break;
	    case "a":
	        shared.cameraDistanceDeltaX = 1;
	        shared.cameraRelativeView = true;
	        break;
	    case "s":
	        shared.cameraDistanceDeltaZ = -1;
	        shared.cameraRelativeView = true;
	        break;
	    case "d":
	        shared.cameraDistanceDeltaX = -1;
	        shared.cameraRelativeView = true;
	        break;

	        //sets what it should move and relative
	    case "W":
	        shared.cameraDistanceDeltaZ = 1;
	        shared.cameraRelativeView = false;
	        break;
	    case "A":
	        shared.cameraDistanceDeltaX = -1;
	        shared.cameraRelativeView = false;
	        break;
	    case "S":
	        shared.cameraDistanceDeltaZ = -1;
	        shared.cameraRelativeView = false;
	        break;
	    case "D":
	        shared.cameraDistanceDeltaX = 1;
	        shared.cameraRelativeView = false;
	        break;

	    case "c":
	    case "C":
	        shared.fourViews = !shared.fourViews;
	        break;

	    case "l":
	    case "L":
	        shared.cameraLight = !shared.cameraLight;
	        break;
	}
}



function keyup(event)
{
    //Stops the camera
	switch (event.key)
    {
	    case "w":
	    case "a":
	    case "s":
	    case "d":
	    case "W":
	    case "A":
	    case "S":
	    case "D":
	        shared.cameraDistanceDeltaZ = 0;
	        shared.cameraDistanceDeltaX = 0;
	        break;
	}
}



function mousemove(event)
{
    //left click
    if (event.buttons == 1) {
        shared.cameraRotationX += -event.movementY * 0.01;
        shared.cameraRotationY += event.movementX * 0.01;
    }

    //Right click
    if (event.buttons == 2)
    {
        shared.cameraRotationZ += event.movementX * 0.01;
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


    //rotates the camera
	rotateCameraX(shared.camera, shared.cameraRotationX);
	rotateCameraY(shared.camera, shared.cameraRotationY);
	rotateCameraZ(shared.camera, shared.cameraRotationZ);

    //moves the camera and if it should move relative to the camera or world
	translateCameraZ(shared.camera, shared.cameraDistanceDeltaZ, shared.cameraRelativeView);
	translateCameraX(shared.camera, shared.cameraDistanceDeltaX, shared.cameraRelativeView);

	updateCamera(shared.camera);

    //gets the width and height of screen
	var width = gl.drawingBufferWidth;
	var height = gl.drawingBufferHeight;

    //Draws only view from main camera over screen
	if (shared.fourViews == false)
	{
	    gl.viewport(0, 0, width, height);

	    mat4.copy(shared.viewMatrix, shared.camera.matrix);
	    mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

	    drawScene(time);
	}


    //Draws view from four diffrent positions and divides the screen in four (one for each camera)
	else if (shared.fourViews == true)
	{
        //main camera
	    gl.viewport(0, 0, width / 2, height / 2);

	    mat4.copy(shared.viewMatrix, shared.camera.matrix);
	    mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

	    drawScene(time);

        //Back camera
	    gl.viewport(width / 2, 0, width / 2, height / 2);

	    mat4.lookAt(shared.viewMatrix, vec3.fromValues(0, 0, 110), vec3.fromValues(0, 0, 0), vec3.fromValues(0, 1, 0));
	    mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

	    drawScene(time);

        //side camera
	    gl.viewport(0, height / 2, width / 2, height / 2);

	    mat4.lookAt(shared.viewMatrix, vec3.fromValues(-85, 55, -85), vec3.fromValues(0, 0, 0), vec3.fromValues(0, 1, 0));
	    mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

	    drawScene(time);

        //top camera
	    gl.viewport(width / 2, height / 2, width / 2, height / 2);

	    mat4.lookAt(shared.viewMatrix, vec3.fromValues(0, 110, 0), vec3.fromValues(0, 0, 0), vec3.fromValues(0, 0, -1));
	    mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

	    drawScene(time);
	}
	


	if (shared.worldMatrixStack.length > 0)
	{
		console.log("worldMatrixStack: Push and pop misalignment"); 
		shared.run = false;
	}

    //Resets main camera rotation so it does not spin ot of control
	shared.cameraRotationX = 0;
	shared.cameraRotationY = 0;
	shared.cameraRotationZ = 0;
}



function drawScene(time)
{
	shared.ambientColor = vec4.fromValues(0.5, 0.5, 0.5, 1);
	shared.lightIntensity = 0.9;

	vec3.copy(shared.cameraPosition, returnCameraPosition(shared.camera));


    //Checks if light source is the camera
	if (shared.cameraLight == false)
	{
	    shared.lightPosition = vec4.fromValues(0, 5, 0, 1);
	}

	else
	{
        //Added a function in camera that returns the position of the camera
	    shared.lightPosition = vec4.fromValues(shared.cameraPosition[0], shared.cameraPosition[1], shared.cameraPosition[2], 1);
	}

	var world = shared.worldMatrix;


	mat4.identity(world);


	pushWorldMatrix();

	mat4.translate(world, world, vec3.fromValues(0, -20, 0));

	setTransformationAndLighting(true);
	gl.bindTexture(gl.TEXTURE_2D, shared.chessboardTexture);
	drawObject(shared.planeObject);

	popWorldMatrix();


	drawCones(35);


	pushWorldMatrix();

    //Draws cube1
	mat4.translate(world, world, vec3.fromValues(20, -15, 35));

	setTransformationAndLighting(true);
	gl.bindTexture(gl.TEXTURE_2D, shared.chessboardTexture);
	drawObject(shared.cubeObject1);

	popWorldMatrix();

    //Draws cube2
	pushWorldMatrix();

	mat4.translate(world, world, vec3.fromValues(-20, -10, -20));

	setTransformationAndLighting(true);
	gl.bindTexture(gl.TEXTURE_2D, shared.chessboardTexture);
	drawObject(shared.cubeObject2);

	popWorldMatrix();



	pushWorldMatrix();

    //If the camera is not the light source, draw a sphere where the light is
	if (shared.cameraLight == false)
	{
	    mat4.translate(world, world, vec3.fromValues(0, 5, 0));
	    setTransformationAndLighting(false);
	    gl.bindTexture(gl.TEXTURE_2D, shared.whiteTexture);
	    drawObject(shared.sphereObject);
	}

	popWorldMatrix();


    //Camera sphere
	pushWorldMatrix();
	mat4.translate(world, world, vec3.fromValues(shared.cameraPosition[0], shared.cameraPosition[1], shared.cameraPosition[2]));
	setTransformationAndLighting(false);
	gl.bindTexture(gl.TEXTURE_2D, shared.whiteTexture);
	drawObject(shared.sphereObject);

	popWorldMatrix();
}



function drawCones(distance)
{
	var world = shared.worldMatrix;

	gl.bindTexture(gl.TEXTURE_2D, shared.chessboardTexture);

	pushWorldMatrix();
	mat4.translate(world, world, vec3.fromValues(-distance, 0, -distance));
	setTransformationAndLighting(true);
	drawObject(shared.coneObject);
	popWorldMatrix();

	pushWorldMatrix();
	mat4.translate(world, world, vec3.fromValues(-distance, 0, distance));
	setTransformationAndLighting(true);
	drawObject(shared.coneObject);
	popWorldMatrix();

	pushWorldMatrix();
	mat4.translate(world, world, vec3.fromValues(distance, 0, -distance));
	setTransformationAndLighting(true);
	drawObject(shared.coneObject);
	popWorldMatrix();

	pushWorldMatrix();
	mat4.translate(world, world, vec3.fromValues(distance, 0, distance));
	setTransformationAndLighting(true);
	drawObject(shared.coneObject);
	popWorldMatrix();
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
