[![Project Status](http://opensource.box.com/badges/active.svg)](http://opensource.box.com/badges)

Extension that allows cost accrual of non-stock item based on line level cost specified in Sales Order, Service Order and Appointment. 
==================================

Out-of-box Acumatica allows cost accrual based on non-stock item’s Standard Cost, Markup Percentage or Percentage of Sales Price. This add-on/extension allows cost accrual based on line level cost defined in Sales Order, Service Order and Appointment. 

### Prerequisites | Supported Versions & Builds ##
* [Acumatica 2020 R1 (20.103.0019 or higher)](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2020R1) 
* [Acumatica 2020 R2 (20.202.0043 or higher)](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2020R2)

Quick Start
-----------

### Installation

##### Install customization deployment package
1. Download the customization package (PXLineCostForAccrue.zip) appropriate for your version of Acumatica.
2. In your Acumatica ERP instance, navigate to System -> Customization -> Customization Projects (SM204505), import PXLineCostForAccrue.zip as a customization project
3. Publish customization project.

### Usage

#### Non-Stock Item (IN202000)

Use Line Cost checkbox has been added to the Non-Stock Items screen (INS202000) -> Price/Cost Information to override out-of-box Accrual Cost Based on behavior. This Field is editable if Accrue Cost is checked.

![Screenshot](/_ReadMeImages/IN202000.png)

When a non-stock item having Use Line Cost checked is used in Sales Order, Service Order and Appointment, Unit Cost field for this line item becomes editable. And user can specify cost. And cost accrual will be based on this line cost value.

#### Sales Order (SO301000)

User can edit Unit Cost value when such item is added. New Column Use Line Cost for Accrue is added for tracking purpose. It will be defaulted from Non-Stock Item -> Use Line Cost.

![Screenshot](/_ReadMeImages/SO301000.png)

When such order is invoiced, it will have cost accrual based on line cost.

#### Service Order (FS300100)

User can edit Unit Cost value when such item is added. New Column Use Line Cost for Accrue is added for tracking purpose. It will be defaulted from Non-Stock Item -> Use Line Cost.

![Screenshot](/_ReadMeImages/FS300100.png)

Invoice created during billing based on service order will have cost accrual based on line item level cost.

#### Appointment (FS300200)

User can edit Unit Cost value when such item is added. New Column Use Line Cost for Accrue is added for tracking purpose. It will be defaulted from Non-Stock Item -> Use Line Cost.

![Screenshot](/_ReadMeImages/FS300200.png)

Invoice created during billing based on appointment will have cost accrual based on line item level cost.

