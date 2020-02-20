import { InjectionToken } from '@angular/core';

export interface _FirbaseUploadConfig {
    firebase: {
        apiKey: string,
        authDomain: string,
        storageBucket: string,
        projectId: string,
    },
    image: {
        height: number,
        width: number,
        quality: number,
    }
}

export const FirbaseUploadConfig = new InjectionToken<_FirbaseUploadConfig>("FirbaseUploadConfig");