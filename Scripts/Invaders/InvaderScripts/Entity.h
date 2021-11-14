#ifndef INCLUDED_ENTITY
#define INCLUDED_ENTITY

#include "VGCVirtualGameConsole.h"
#include <string>

class Entity {
public:
	Entity();
	virtual ~Entity();
	virtual void update() = 0;
	virtual void render() = 0;
	virtual VGCVector getPosition() const = 0;
	virtual int getSize() const = 0;
	virtual void destroy(Entity *ent) = 0;
	virtual std::string getType() const = 0;
};

#endif