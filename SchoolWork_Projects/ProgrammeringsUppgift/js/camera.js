/*

 createCamera(): Initierar kameran

 rotateCameraX(): Roterar kameran kring x-axeln
 rotateCameraY(): Roterar kameran kring y-axeln
 rotateCameraZ(): Roterar kameran kring z-axeln

 translateCameraX(): Translerar kameran utmed x-axeln
 translateCameraY(): Translerar kameran utmed y-axeln
 translateCameraZ(): Translerar kameran utmed z-axeln

 updateCamera(): Beräknar transformationsmatrisen för kameran

*/



function createCamera(positionX, positionY, positionZ)
{
	var camera = {};

	camera.matrix = mat4.create();

	camera.position = vec3.fromValues(positionX, positionY, positionZ);

	camera.right = vec3.create();
	camera.up = vec3.create();
	camera.forward = vec3.fromValues(0, 0, 1);
	camera.upReference = vec3.fromValues(0, 1, 0);

	camera.temporaryVector = vec3.create();

	return camera;
}



function rotateCameraX(camera, angle)
{
	vec3.set(camera.temporaryVector, 0, 0, 0);
	vec3.rotateX(camera.forward, camera.forward, camera.temporaryVector, angle);
	updateRightUpVectors(camera);
}



function rotateCameraY(camera, angle)
{
	vec3.set(camera.temporaryVector, 0, 0, 0);
	vec3.rotateY(camera.forward, camera.forward, camera.temporaryVector, angle);
	updateRightUpVectors(camera);
}



function rotateCameraZ(camera, angle)
{
	vec3.set(camera.temporaryVector, 0, 0, 0);
	vec3.rotateZ(camera.upReference, camera.upReference, camera.temporaryVector, angle);
	updateRightUpVectors(camera);
}



function translateCameraX(camera, amount, viewRelative)
{
	if (viewRelative)
	{
		vec3.copy(camera.temporaryVector, camera.right);
		camera.temporaryVector[0] = -camera.temporaryVector[0];
		vec3.scaleAndAdd(camera.position, camera.position, camera.temporaryVector, amount);
	}
	else
	{
		vec3.set(camera.temporaryVector, -amount, 0, 0);
		vec3.add(camera.position, camera.position, camera.temporaryVector);
	}
}



function translateCameraY(camera, amount, viewRelative)
{
	if (viewRelative)
	{
		vec3.scaleAndAdd(camera.position, camera.position, camera.up, amount);
	}
	else
	{
		vec3.set(camera.temporaryVector, 0, amount, 0);
		vec3.add(camera.position, camera.position, camera.temporaryVector);
	}
}



function translateCameraZ(camera, amount, viewRelative)
{
	if (viewRelative)
	{
		vec3.copy(camera.temporaryVector, camera.forward);
		camera.temporaryVector[0] = -camera.temporaryVector[0];
		vec3.scaleAndAdd(camera.position, camera.position, camera.temporaryVector, amount);
	}
	else
	{
		vec3.set(camera.temporaryVector, 0, 0, amount);
		vec3.add(camera.position, camera.position, camera.temporaryVector);
	}
}



function updateCamera(camera)
{
	updateRightUpVectors(camera);

	mat4.identity(camera.matrix);

	camera.matrix[0] = camera.right[0];
	camera.matrix[1] = camera.right[1];
	camera.matrix[2] = camera.right[2];
	camera.matrix[4] = camera.up[0];
	camera.matrix[5] = camera.up[1];
	camera.matrix[6] = camera.up[2];
	camera.matrix[8] = -camera.forward[0];
	camera.matrix[9] = -camera.forward[1];
	camera.matrix[10] = -camera.forward[2];

	vec3.negate(camera.temporaryVector, camera.position);
	mat4.translate(camera.matrix, camera.matrix, camera.temporaryVector);
}



function updateRightUpVectors(camera)
{
	vec3.cross(camera.right, camera.forward, camera.upReference);
	vec3.normalize(camera.right, camera.right);
	vec3.cross(camera.up, camera.right, camera.forward);
	vec3.normalize(camera.up, camera.up);
}


function returnCameraPosition(camera)
{
    return camera.position;
}
