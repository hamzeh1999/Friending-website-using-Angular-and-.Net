import { Photo } from "./Photo"

export interface Memeber {
    id: number;
    userName: string;
    created: Date;
    lastActivity: Date;
    age: number;
    knownAs: string;
    photoUrl: string;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    country: string;
    photos: Photo[];
    city: string;
  }
  
