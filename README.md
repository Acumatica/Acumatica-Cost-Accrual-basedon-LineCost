[![Project Status](http://opensource.box.com/badges/active.svg)](http://opensource.box.com/badges)

Extension that allows cost accrual of non-stock item based on line level cost specified in Sales Order, Service Order and Appointment. 
==================================

Out-of-box Acumatica allows cost accrual based on non-stock item’s Standard Cost, Markup Percentage or Percentage of Sales Price. This add-on/extension allows cost accrual based on line level cost defined in Sales Order, Service Order and Appointment. 

### Prerequisites | Supported Versions & Builds ##
* Acumatica 2020 R1 (20.103.0019 or higher) [2020 R1 Source and Deployment Package](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2020R1)
* Acumatica 2020 R2 (20.202.0043 or higher) [2020 R2 Source and Deployment Package](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2020R2)
* Acumatica 2021 R2 (21.207.0045 or higher) [2021 R2 Source and Deployment Package](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2021R2)
* Acumatica 2023 R2 (23.212.0024 or higher) [2023 R2 Source and Deployment Package](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2023R2)
* Acumatica 2024 R2 (24.208.0020 or higher) [2024 R2 Source and Deployment Package](https://github.com/Acumatica/Acumatica-Cost-Accrual-basedon-LineCost/tree/2024R2)

Quick Start
-----------

### Installation

##### Install customization deployment package
1. Download the customization package (PXLineCostForAccrue.zip) appropriate for your version of Acumatica.
2. In your Acumatica ERP instance, navigate to System -> Customization -> Customization Projects (SM204505), import PXLineCostForAccrue.zip as a customization project
3. Publish customization project.

### Usage

#### Non-Stock Item (IN202000)

Use Line Cost checkbox has been added to the Non-Stock Items screen (INS202000) -> Price/Cost Information to override out-of-box Accrual Cost Based on behavior. This field is editable if Accrue Cost is checked.

![Screenshot](/_ReadMeImages/IN202000.png)

When a non-stock item having Use Line Cost checked is used in Sales Order, Service Order and Appointment, Unit Cost field for this line item becomes editable. And user can specify cost. And cost accrual will be based on this line cost value.

#### Sales Order (SO301000)

User can edit Unit Cost value when such item is added. New Column Use Line Cost for Accrue is added for tracking purpose. It will be defaulted from Non-Stock Item -> Use Line Cost.

![Screenshot](/_ReadMeImages/SO301000.png)

When such order is invoiced, it will have cost accrual based on sales order line cost.

#### Service Order (FS300100)

User can edit Unit Cost value when such item is added. New Column Use Line Cost for Accrue is added for tracking purpose. It will be defaulted from Non-Stock Item -> Use Line Cost.

![Screenshot](/_ReadMeImages/FS300100.png)

Invoice created during billing based on service order will have cost accrual based on service order line item level cost.

#### Appointment (FS300200)

User can edit Unit Cost value when such item is added. New Column Use Line Cost for Accrue is added for tracking purpose. It will be defaulted from Non-Stock Item -> Use Line Cost.

![Screenshot](/_ReadMeImages/FS300200.png)

Invoice created during billing based on appointment will have cost accrual based on appointment line item level cost.

Known Issues
------------
None at the moment

## Copyright and License

Copyright © `2020` `Acumatica, INC`

This component is licensed under the MIT License, a copy of which is available online [here](LICENSE)
