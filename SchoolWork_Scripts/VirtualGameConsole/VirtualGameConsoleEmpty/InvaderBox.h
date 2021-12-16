#ifndef INCLUDED_INVADERBOX
#define INCLUDED_INVADERBOX

#include "Entity.h"
#include "VGCVirtualGameConsole.h"
#include <string>
#include "Vectors.h"

class InvaderBox : public Entity {
public:
	InvaderBox(int fps, Vectors *vec);
	virtual ~InvaderBox();
	virtual void update();
	virtual void render();
	static void initialize();
	static void finalize();
	virtual void destroy(Entity *ent);

	VGCVector getPosition() const;
	int getSize() const;
private:
	VGCVector mPosition;
	Vectors *vector;
};

#endif