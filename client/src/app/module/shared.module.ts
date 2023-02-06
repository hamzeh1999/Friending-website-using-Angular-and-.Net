import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@rybos/ngx-gallery';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
// import{ timeagoSimple } from '../../../node_modules/timeago-simple'
import { MomentModule } from 'ngx-moment';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MomentModule.forRoot({
      relativeTimeThresholdOptions: {
        'm': 59,
        's':59,
      }
    }),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    ButtonsModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 1000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: false,    }),
      TabsModule.forRoot(),
      NgxGalleryModule,
         

  ],
  exports:[
    TabsModule,
    NgxGalleryModule,
    BsDatepickerModule,
    PaginationModule,
    ButtonsModule,
    MomentModule

  ]
})
export class SharedModule { }
