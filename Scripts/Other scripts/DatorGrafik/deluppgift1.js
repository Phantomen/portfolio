var canvas = document.createElement("canvas");

var gl = canvas.getContext("webgl");


var shared =
{
    worldMatrix: mat4.create(),
	viewMatrix: mat4.create(),
	projectionMatrix: mat4.create(),
	viewProjectionMatrix: mat4.create(),
	worldViewProjectionMatrix: mat4.create(),

	worldViewProjectionMatrixLocation: null,
	vertexPositionLocation: null,
	vertexColorLocation: null,

	time: 0,
	previousTime: 0,

    cameraPosition: vec3.create(),
    plane1Position: vec3.create(),
    plane2Position: vec3.create(),

    square: { positionBuffer: null, colorBuffer: null, triangleCount: 0 },

    house1: { positionBuffer: null, colorBuffer: null, triangleCount: 0 },
    house2: { positionBuffer: null, colorBuffer: null, indexBuffer: null, triangleCount: 0 },

    plane1: { positionBuffer: null, colorBuffer: null, indexBuffer: null, triangleCount: 0 },
    plane2: { positionBuffer: null, colorBuffer: null, indexBuffer: null, triangleCount: 0 },

    paused: false,

    backfaceCulling: false,

    zBuffer: false,

    over: false
};



function main(context)
{
	gl = context;


	window.addEventListener("keydown", keydown);
	gl.canvas.addEventListener("mousemove", mousemove);

	var program = initializeProgram(vertexShader, fragmentShader);
	if (!program)
	{
		window.removeEventListener("keydown", keydown);
		gl.canvas.removeEventListener("mousemove", mousemove);
		return;
	}

	gl.useProgram(program);
	shared.worldViewProjectionMatrixLocation = gl.getUniformLocation(program, "u_worldViewProjection");
	shared.vertexPositionLocation = gl.getAttribLocation(program, "a_position");
	shared.vertexColorLocation = gl.getAttribLocation(program, "a_color");
	gl.enableVertexAttribArray(shared.vertexPositionLocation);
	gl.enableVertexAttribArray(shared.vertexColorLocation);

	var aspectRatio = gl.drawingBufferWidth / gl.drawingBufferHeight;
	mat4.perspective(shared.projectionMatrix, Math.PI/4, aspectRatio, 1, 150);

	initializeScene();


	window.requestAnimationFrame(frameCallback);
}



function initializeScene()
{
    createSquare();
    createHouse1();
    createHouse2();
    createPlane1();
    createPlane2();
}



function createSquare()
{
	var positions = [-20,0,-10, -20,0,10, 20,0,-10, -20,0,10, 20,0,10, 20,0,-10];
	var colors = [0,1,0,1, 0,1,0,1, 0,1,0,1, 0,1,0,1, 0,1,0,1, 0,1,0,1];

	shared.square.positionBuffer = gl.createBuffer();
	gl.bindBuffer(gl.ARRAY_BUFFER, shared.square.positionBuffer);
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);

	shared.square.colorBuffer = gl.createBuffer();
	gl.bindBuffer(gl.ARRAY_BUFFER, shared.square.colorBuffer);
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

    shared.square.triangleCount = positions.length / 3;
}


function createHouse1()
{
    //Every vertex
    var bottomHeight = [-5];
    var mediumHeight = [0];
    var topHeight = [5];

    var xPositionP = [5];
    var xPositionN = [-5];
    var xPositionM = [0];
    var zPosition = [5];

    //Adds vertex to list to create triangles
    var positions = [
        xPositionN, mediumHeight, zPosition,
        xPositionN, bottomHeight, zPosition,
        xPositionP, bottomHeight, zPosition,

        xPositionP, bottomHeight, zPosition,
        xPositionP, mediumHeight, zPosition,
        xPositionN, mediumHeight, zPosition,


        xPositionN, bottomHeight, -zPosition,
        xPositionN, mediumHeight, -zPosition,
        xPositionP, bottomHeight, -zPosition,

        xPositionP, mediumHeight, -zPosition,
        xPositionP, bottomHeight, -zPosition,
        xPositionN, mediumHeight, -zPosition,


        xPositionN, bottomHeight, -zPosition,
        xPositionN, bottomHeight, zPosition,
        xPositionN, mediumHeight, -zPosition,

        xPositionN, bottomHeight, zPosition,
        xPositionN, mediumHeight, zPosition,
        xPositionN, mediumHeight, -zPosition,


        xPositionP, bottomHeight, zPosition,
        xPositionP, bottomHeight, -zPosition,
        xPositionP, mediumHeight, -zPosition,

        xPositionP, mediumHeight, zPosition,
        xPositionP, bottomHeight, zPosition,
        xPositionP, mediumHeight, -zPosition,


        xPositionP, bottomHeight, -zPosition,
        xPositionP, bottomHeight, zPosition,
        xPositionN, bottomHeight, zPosition,

        xPositionN, bottomHeight, -zPosition,
        xPositionP, bottomHeight, -zPosition,
        xPositionN, bottomHeight, zPosition,


        xPositionN, mediumHeight, zPosition,
        xPositionP, mediumHeight, zPosition,
        xPositionM, topHeight, zPosition,

        xPositionP, mediumHeight, -zPosition,
        xPositionN, mediumHeight, -zPosition,
        xPositionM, topHeight, -zPosition,


        xPositionN, mediumHeight, -zPosition,
        xPositionN, mediumHeight, zPosition,
        xPositionM, topHeight, zPosition,

        xPositionM, topHeight, -zPosition,
        xPositionN, mediumHeight, -zPosition,
        xPositionM, topHeight, zPosition,


        xPositionP, mediumHeight, zPosition,
        xPositionP, mediumHeight, -zPosition,
        xPositionM, topHeight, zPosition,

        xPositionP, mediumHeight, -zPosition,
        xPositionM, topHeight, -zPosition,
        xPositionM, topHeight, zPosition
    ];

    //Every color
    var redColor = [1, 0, 0, 1];
    var greenColor = [0, 1, 0, 1];
    var blueColor = [0, 0, 1, 1];
    var yellowColor = [1, 1, 0, 1];
    var purpleColor = [1, 0, 1, 1];

    //Adds colors to list to every vertex
    var c = [
        purpleColor, greenColor, yellowColor,
        yellowColor, blueColor, purpleColor,

        blueColor, yellowColor, redColor,
        purpleColor, redColor, yellowColor,

        blueColor, greenColor, yellowColor,
        greenColor, purpleColor, yellowColor,

        yellowColor, redColor, purpleColor,
        blueColor, yellowColor, purpleColor,

        redColor, yellowColor, greenColor,
        blueColor, redColor, greenColor,

        purpleColor, blueColor, greenColor,
        purpleColor, yellowColor, redColor,

        yellowColor, purpleColor, greenColor,
        redColor, yellowColor, greenColor,

        blueColor, purpleColor, greenColor,
        purpleColor, redColor, greenColor
    ];

    //Sets the color list
    var colors = [].concat.apply([], c);



    shared.house1.positionBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house1.positionBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);

    shared.house1.colorBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house1.colorBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

    shared.house1.triangleCount = positions.length / 3;
}

function createHouse2() {
    var bottomHeight = [-5];
    var mediumHeight = [0];
    var topHeight = [5];

    var xPositionP = [5];
    var xPositionN = [-5];
    var xPositionM = [0];
    var zPosition = [5];


    var positions = [
        xPositionN, mediumHeight, zPosition,
        xPositionN, bottomHeight, zPosition,
        xPositionP, bottomHeight, zPosition,
        xPositionP, mediumHeight, zPosition,

        xPositionN, bottomHeight, -zPosition,
        xPositionN, mediumHeight, -zPosition,
        xPositionP, bottomHeight, -zPosition,
        xPositionP, mediumHeight, -zPosition,

        xPositionM, topHeight, zPosition,
        xPositionM, topHeight, -zPosition
    ];

    //Sets cornerlist
    var cornerList = [].concat.apply([], positions);

    //index list
    var indexes = [
        0, 1, 2,
        2, 3, 0,

        4, 5, 6,
        7, 6, 5,

        4, 1, 5,
        1, 0, 5,

        2, 6, 7,
        3, 2, 7,

        6, 2, 1,
        4, 6, 1,

        0, 3, 8,

        7, 5, 9,

        5, 0, 8,
        9, 5, 8,

        3, 7, 8,
        7, 9, 8
    ];

    var redColor = [1, 0, 0, 1];
    var greenColor = [0, 1, 0, 1];
    var blueColor = [0, 0, 1, 1];
    var yellowColor = [1, 1, 0, 1];
    var purpleColor = [1, 0, 1, 1];

    var c = [
        purpleColor,
        greenColor,
        yellowColor,
        blueColor,

        blueColor,
        yellowColor,
        redColor,
        purpleColor,

        greenColor,
        redColor
    ];

    var colors = [].concat.apply([], c);



    shared.house2.positionBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house2.positionBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);

    shared.house2.colorBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house2.colorBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

    shared.house2.indexBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, shared.house2.indexBuffer);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indexes), gl.STATIC_DRAW);

    shared.house2.indexCount = indexes.length;
}


function createPlane1()
{
    var bottomHeight = [0];
    var midHeight = [20];
    var topHeight = [30];

    var xPosition = [0];

    var zPositionP = [20];
    var zPositionN = [-20];

    var positions = [
        xPosition, bottomHeight, zPositionN,
        xPosition, midHeight, zPositionN,
        xPosition, topHeight, zPositionN,

        xPosition, bottomHeight, zPositionP,
        xPosition, midHeight, zPositionP,
        xPosition, topHeight, zPositionP
    ];

    var indexes = [
        0, 3, 1,
        1, 3, 4,

        1, 4, 2,
        2, 4, 5
    ];

    var blue = [0, 1, 1, 0.8];
    var fadingBlue = [0, 1, 1, 0.25];
    var fadedBlue = [0, 1, 1, 0];

    var c = [
        blue,
        fadingBlue,
        fadedBlue,
        blue,
        fadingBlue,
        fadedBlue
    ];

    var colors = [].concat.apply([], c);


    shared.plane1.positionBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane1.positionBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);

    shared.plane1.colorBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane1.colorBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

    shared.plane1.indexBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, shared.plane1.indexBuffer);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indexes), gl.STATIC_DRAW);

    shared.plane1.indexCount = indexes.length;

    vec3.set(shared.plane1Position, xPosition, bottomHeight / 2 + topHeight / 2, zPositionN + zPositionP);
}

function createPlane2() {
    var bottomHeight = [0];
    var midHeight = [20];
    var topHeight = [30];

    var xPosition = [0];

    var zPositionP = [20];
    var zPositionN = [-20];

    var positions = [
        xPosition, bottomHeight, zPositionN,
        xPosition, midHeight, zPositionN,
        xPosition, topHeight, zPositionN,

        xPosition, bottomHeight, zPositionP,
        xPosition, midHeight, zPositionP,
        xPosition, topHeight, zPositionP
    ];

    var indexes = [
        0, 3, 1,
        1, 3, 4,

        1, 4, 2,
        2, 4, 5
    ];

    var purple = [1, 0, 1, 0.8];
    var fadingPurple = [1, 0, 1, 0.25];
    var fadedPurple = [1, 0, 1, 0];

    var c = [
        purple,
        fadingPurple,
        fadedPurple,
        purple,
        fadingPurple,
        fadedPurple
    ];

    var colors = [].concat.apply([], c);


    shared.plane2.positionBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane2.positionBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);

    shared.plane2.colorBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane2.colorBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

    shared.plane2.indexBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, shared.plane2.indexBuffer);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indexes), gl.STATIC_DRAW);

    shared.plane2.indexCount = indexes.length;

    vec3.set(shared.plane2Position ,xPosition, bottomHeight / 2 + topHeight / 2, zPositionN + zPositionP);
}


function frameCallback(time)
{
	var deltaTime = time - shared.previousTime;
	if (!shared.paused) shared.time += deltaTime;
	shared.previousTime = time;

	frame(shared.time * 0.001);

	window.requestAnimationFrame(frameCallback);
}



function keydown(event)
{
	if (event.key == "p")
        shared.paused = !shared.paused;

    if (event.key == "b")
    {
        if (shared.backfaceCulling == true)
            gl.disable(gl.CULL_FACE);

        else
            gl.enable(gl.CULL_FACE);

        shared.backfaceCulling = !shared.backfaceCulling;
        gl.cullFace(gl.BACK);
    }


    if (event.key == "z")
    {
        if (shared.zBuffer == true)
            gl.disable(gl.DEPTH_TEST);

        else
            gl.enable(gl.DEPTH_TEST);

        shared.zBuffer = !shared.zBuffer;
        gl.depthFunc(gl.LESS);
    }

    if (event.key == "a")
    {
        shared.over = !shared.over;
    }
}



function mousemove(event)
{
}



function setWorldViewProjection()
{
	mat4.multiply(shared.worldViewProjectionMatrix, shared.viewProjectionMatrix, shared.worldMatrix);
	gl.uniformMatrix4fv(shared.worldViewProjectionMatrixLocation, false, shared.worldViewProjectionMatrix);
}



function frame(time)
{
	gl.clearColor(0, 0, 0, 1);
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

	vec3.set(shared.cameraPosition, Math.cos(time)*80, 0, Math.sin(time)*80);
	mat4.lookAt(shared.viewMatrix, shared.cameraPosition, vec3.fromValues(0, 0, 0), vec3.fromValues(0, 1, 0));
    mat4.multiply(shared.viewProjectionMatrix, shared.projectionMatrix, shared.viewMatrix);

    drawScene(time);
}



function drawScene(time)
{
	var world = shared.worldMatrix;

	mat4.identity(world);
	mat4.translate(world, world, vec3.fromValues(0, -20, 0));

	setWorldViewProjection();
    drawSquare();

    var degrees = 50.0;


    mat4.identity(world);
    mat4.translate(world, world, vec3.fromValues(0, -5, 10));
    mat4.rotate(world, world, time * degrees / 180 * Math.PI, vec3.fromValues(0, 0, 1));
    setWorldViewProjection();

    drawHouse1();


    mat4.identity(world);
    mat4.translate(world, world, vec3.fromValues(0, -5, -10));
    mat4.rotate(world, world, time * degrees / 180 * Math.PI, vec3.fromValues(0, 0, 1));
    setWorldViewProjection();

    drawHouse2();


    

    mat4.identity(world);

    vec3.add(shared.plane1Position, shared.plane1Position, vec3.fromValues(20, -20, 0));
    vec3.add(shared.plane2Position, shared.plane2Position, vec3.fromValues(-20, -20, 0));

    //Draws the windows in the right order depending on distance from the camera
    if ((vec3.distance(shared.cameraPosition, shared.plane1Position))
        < (vec3.distance(shared.cameraPosition, shared.plane2Position)))
    {
        mat4.translate(world, world, vec3.fromValues(-20, -20, 0));

        setWorldViewProjection();

        gl.enable(gl.BLEND);

        //Sets type of transparancy
        if (shared.over == true)
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);

        else
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE);


        drawPlane2();

        gl.disable(gl.BLEND);


        mat4.identity(world);
        mat4.translate(world, world, vec3.fromValues(20, -20, 0));

        setWorldViewProjection();

        gl.enable(gl.BLEND);

        if (shared.over == true)
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);

        else
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE);


        drawPlane1();

        gl.disable(gl.BLEND);
    }

    else
    {
        mat4.translate(world, world, vec3.fromValues(20, -20, 0));

        setWorldViewProjection();

        gl.enable(gl.BLEND);

        if (shared.over == true)
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);

        else
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE);

        drawPlane1();

        gl.disable(gl.BLEND);


        mat4.identity(world);
        mat4.translate(world, world, vec3.fromValues(-20, -20, 0));

        setWorldViewProjection();

        gl.enable(gl.BLEND);

        if (shared.over == true)
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA);

        else
            gl.blendFunc(gl.SRC_ALPHA, gl.ONE);

        drawPlane2();

        gl.disable(gl.BLEND);        
    }
}



function drawSquare()
{
	gl.bindBuffer(gl.ARRAY_BUFFER, shared.square.positionBuffer);
	gl.vertexAttribPointer(shared.vertexPositionLocation, 3, gl.FLOAT, gl.FALSE, 0, 0);

	gl.bindBuffer(gl.ARRAY_BUFFER, shared.square.colorBuffer);
	gl.vertexAttribPointer(shared.vertexColorLocation, 4, gl.FLOAT, gl.FALSE, 0, 0);

	gl.drawArrays(gl.TRIANGLES, 0, shared.square.triangleCount);
}


function drawHouse1()
{
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house1.positionBuffer);
    gl.vertexAttribPointer(shared.vertexPositionLocation, 3, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house1.colorBuffer);
    gl.vertexAttribPointer(shared.vertexColorLocation, 4, gl.FLOAT, gl.FALSE, 0, 0);

    gl.drawArrays(gl.TRIANGLES, 0, shared.house1.triangleCount);
}

function drawHouse2()
{
    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house2.positionBuffer);
    gl.vertexAttribPointer(shared.vertexPositionLocation, 3, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ARRAY_BUFFER, shared.house2.colorBuffer);
    gl.vertexAttribPointer(shared.vertexColorLocation, 4, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, shared.house2.indexBuffer);

    gl.drawElements(gl.TRIANGLES, shared.house2.indexCount, gl.UNSIGNED_SHORT, 0);
}

function drawPlane1()
{
    //Disables backfaceculling if enabled
    if (shared.backfaceCulling == true)
        gl.disable(gl.CULL_FACE);


    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane1.positionBuffer);
    gl.vertexAttribPointer(shared.vertexPositionLocation, 3, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane1.colorBuffer);
    gl.vertexAttribPointer(shared.vertexColorLocation, 4, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, shared.plane1.indexBuffer);

    gl.drawElements(gl.TRIANGLES, shared.plane1.indexCount, gl.UNSIGNED_SHORT, 0);


    //Turns backfaceculling back on if it is "enabled"
    if (shared.backfaceCulling == true)
        gl.enable(gl.CULL_FACE);
}

function drawPlane2() {
    if (shared.backfaceCulling == true)
        gl.disable(gl.CULL_FACE);


    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane2.positionBuffer);
    gl.vertexAttribPointer(shared.vertexPositionLocation, 3, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ARRAY_BUFFER, shared.plane2.colorBuffer);
    gl.vertexAttribPointer(shared.vertexColorLocation, 4, gl.FLOAT, gl.FALSE, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, shared.plane2.indexBuffer);

    gl.drawElements(gl.TRIANGLES, shared.plane2.indexCount, gl.UNSIGNED_SHORT, 0);



    if (shared.backfaceCulling == true)
        gl.enable(gl.CULL_FACE);
}



var vertexShader =
`
	uniform mat4 u_worldViewProjection;
	attribute vec4 a_position;
	attribute vec4 a_color;
	varying vec4 v_color;

	void main(void)
	{
		v_color = a_color;
		gl_Position = u_worldViewProjection * a_position;
	}
`;



var fragmentShader =
`
	varying highp vec4 v_color;

	void main(void)
	{
		gl_FragColor = v_color;
	}
`;
